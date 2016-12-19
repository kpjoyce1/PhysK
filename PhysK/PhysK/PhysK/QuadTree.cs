using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhysK
{
    public class QuadTree
    {
        private int maxObjects;

        public int MaxObjects
        {
            get { return maxObjects; }
        }

        private int maxLevel;

        public int MaxLevel
        {
            get { return maxLevel; }
        }


        private int level;

        public int Level
        {
            get { return level; }
        }

        private float mass;

        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        private float charge;

        public float Charge
        {
            get { return charge; }
            set { charge = value; }
        }


        private Vector2 massPosition;

        public Vector2 MassPosition
        {
            get { return massPosition; }
            set { massPosition = value; }
        }

        private Vector2 chargePosition;

        public Vector2 ChargePosition
        {
            get { return chargePosition; }
            set { chargePosition = value; }
        }


        public Vector2 CenterOfCharge => this.chargePosition / this.charge;
        public Vector2 CenterOfMass => this.massPosition / this.mass;


        private List<Particle> objects;

        private RectangleF bounds;

        public RectangleF Bounds
        {
            get { return bounds; }
        }

        private QuadTree[] nodes;
        public static Texture2D Pixel;

        public QuadTree(int level, RectangleF bounds)
        {
            this.level = level;
            this.objects = new List<Particle>();
            this.bounds = bounds;
            this.nodes = new QuadTree[4];
            this.maxLevel = 50;
            this.maxObjects = 1;
            this.mass = 0;
        }

        public void Clear()
        {
            objects = new List<Particle>();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].mass = 0f;
                    nodes[i].massPosition = Vector2.Zero;
                    nodes[i].charge = 0f;
                    nodes[i].chargePosition = Vector2.Zero;
                    nodes[i].Clear();
                    nodes[i] = null;
                }
            }

        }

        private void split()
        {
            float subWidth = bounds.Width / 2f;
            float subHeight = bounds.Height / 2f;
            float x = bounds.X;
            float y = bounds.Y;
            
            nodes[0] = new QuadTree(level + 1, new RectangleF(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new QuadTree(level + 1, new RectangleF(x, y, subWidth, subHeight));
            nodes[2] = new QuadTree(level + 1, new RectangleF(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new QuadTree(level + 1, new RectangleF(x + subWidth, y + subHeight, subWidth, subHeight));

        }

        private int getIndex(Particle entity)
        {
            int index = -1;
            double verticalMidpoint = bounds.X + (bounds.Width / 2);
            double horizontalMidpoint = bounds.Y + (bounds.Height / 2);

            if (entity is Rigidbody)
            {
                Rigidbody temp = entity as Rigidbody;
                bool topQuadrant = (entity.Position.Y < horizontalMidpoint && entity.Position.Y + temp.Shape.AABB.Height < horizontalMidpoint);

                bool bottomQuadrant = (entity.Position.Y > horizontalMidpoint);


                if (entity.Position.X < verticalMidpoint && entity.Position.X + temp.Shape.AABB.Width < verticalMidpoint)
                {
                    if (topQuadrant)
                    {
                        index = 1;
                    }
                    else if (bottomQuadrant)
                    {
                        index = 2;
                    }
                }

                else if (entity.Position.X > verticalMidpoint)
                {
                    if (topQuadrant)
                    {
                        index = 0;
                    }
                    else if (bottomQuadrant)
                    {
                        index = 3;
                    }
                }
            }
            else
            {
                bool topQuadrant = (entity.Position.Y < horizontalMidpoint);

                bool bottomQuadrant = (entity.Position.Y > horizontalMidpoint);


                if (entity.Position.X < verticalMidpoint)
                {
                    if (topQuadrant)
                    {
                        index = 1;
                    }
                    else if (bottomQuadrant)
                    {
                        index = 2;
                    }
                }

                else if (entity.Position.X > verticalMidpoint)
                {
                    if (topQuadrant)
                    {
                        index = 0;
                    }
                    else if (bottomQuadrant)
                    {
                        index = 3;
                    }
                }
            }

            return index;
        }


        public void Insert(Particle entity)
        {
            if (nodes[0] != null)
            {
                int index = getIndex(entity);

                if (index != -1)
                {
                    this.mass += entity.Mass;
                    this.charge += entity.Charge;
                    this.massPosition += entity.MassPosition;
                    this.chargePosition += chargePosition;
                    nodes[index].Insert(entity);

                    return;     
                }
            }

            string tempRank = "AriaIsSickOfGrindingSendHelpThanks" + "";

            this.mass += entity.Mass;
            this.charge += entity.Charge;
            this.massPosition += entity.MassPosition;
            this.chargePosition += chargePosition;
            objects.Add(entity);

            if (objects.Count > this.maxObjects && level < this.maxLevel)
            {
                if (nodes[0] == null)
                {
                    split();
                }

                int i = 0;
                while (i < objects.Count)
                {
                    int index = getIndex(objects[i]);
                    if (index != -1)
                    {
                        nodes[index].Insert(objects[i]);
                        objects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

            }
        }

        public Vector2 GetEffectiveCoulumbForce(Vector2 effectiveCoulumb, Particle entity)
        {
            int index = getIndex(entity);
            if (index != -1 && nodes[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i != index && nodes[i].charge != 0)
                    {
                        Vector2 direction = nodes[i].CenterOfCharge - entity.Position;
                        direction.Normalize();
                        Vector2 coulumbForce = World.CoulumbConstant * direction * entity.Charge * nodes[i].Charge / (float)Math.Sqrt(Math.Pow(nodes[i].CenterOfCharge.Y - entity.Position.Y, 2) + Math.Pow(nodes[i].CenterOfCharge.X - entity.Position.X, 2));
                        effectiveCoulumb += coulumbForce;
                    }
                }
                nodes[index].GetEffectiveGravity(effectiveCoulumb, entity);
                return effectiveCoulumb;
            }

            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].Charge != 0 && objects[i].Position != entity.Position)
                {
                    Vector2 direction = objects[i].Position - entity.Position; //center of mass - item position
                    direction.Normalize();
                    Vector2 coulumbForce = World.CoulumbConstant * direction * entity.Charge * objects[i].Charge / (float)Math.Sqrt(Math.Pow(objects[i].Position.Y - entity.Position.Y, 2) + Math.Pow(objects[i].Position.X - entity.Position.X, 2));
                    effectiveCoulumb += coulumbForce;
                }
            }
            return effectiveCoulumb;


        }
      

        public Vector2 GetEffectiveGravity(Vector2 effectiveGravity, Particle entity)
        {
            int index = getIndex(entity);
            if(index != -1 && nodes[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if(i != index && nodes[i].mass != 0)
                    {
                        Vector2 direction = nodes[i].CenterOfMass - entity.Position; 
                        direction.Normalize();
                        Vector2 GravitationalForce = World.GravitationalConstant * direction * entity.Mass * nodes[i].Mass / (float)Math.Sqrt(Math.Pow(nodes[i].CenterOfMass.Y - entity.Position.Y, 2) + Math.Pow(nodes[i].CenterOfMass.X - entity.Position.X, 2));
                        effectiveGravity += GravitationalForce;
                    }
                }
                nodes[index].GetEffectiveGravity(effectiveGravity, entity);
                return effectiveGravity;
            }

            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].Mass != 0 && objects[i].Position != entity.Position)
                {
                    Vector2 direction = objects[i].Position - entity.Position; //center of mass - item position
                    direction.Normalize();
                    Vector2 GravitationalForce = World.GravitationalConstant * direction * entity.Mass * objects[i].Mass / (float)Math.Sqrt(Math.Pow(objects[i].Position.Y - entity.Position.Y, 2) + Math.Pow(objects[i].Position.X - entity.Position.X, 2));
                    effectiveGravity += GravitationalForce;
                }
            }
            return effectiveGravity;

        }

        public List<Particle> Retrieve(List<Particle> returnEntities, Particle entity)
        {

            int index = getIndex(entity);
            if(index != -1 && nodes[0] != null)
            {
                nodes[index].Retrieve(returnEntities, entity);
            }
            
            for(int i = 0; i < objects.Count;i++)
            {
                returnEntities.Add(objects[i]);
            }

            return returnEntities;

        }

    }


}

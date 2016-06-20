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
            this.maxLevel = 10;
            this.maxObjects = 20;
        }

        public void Clear()
        {
            objects = new List<Particle>();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
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
                    nodes[index].Insert(entity);

                    return;     
                }
            }

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

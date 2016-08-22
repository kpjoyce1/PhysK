using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysK
{
    public class Particle
    {
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private Vector2 velocity;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        private Vector2 acceleration;

        public Vector2 Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        private List<Vector2> forces;

        public List<Vector2> Forces
        {
            get { return forces; }
            set { forces = value; }
        }

        private List<Vector2> quadTreeForces;

        public List<Vector2> QuadTreeForces
        {
            get { return quadTreeForces; }
            set { quadTreeForces = value; }
        }
        
        protected float mass;

        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        protected float charge;

        public float Charge
        {
            get { return charge; }
            set { charge = value; }
        }


        private float restitution;

        public float Restitution
        {
            get { return restitution; }
            set { restitution = value; }
        }

        public bool Sleep;
        public int FramesAtRest;
        public bool JustHit;
        public Vector2 MassPosition => position * mass;
        public Vector2 ChargePosition => position * charge;

        public Particle(Vector2 position, Vector2 velocity, float mass, float charge, float restitution)
        {
            this.position = position;
            this.velocity = velocity;
            this.mass = mass;
            this.charge = charge;
            this.restitution = restitution;
            forces = new List<Vector2>();
            quadTreeForces = new List<Vector2>();
        }

        private float friction = 0.001f;

        public virtual void Update(GameTime gameTime)
        {
            acceleration = Vector2.Zero;
            foreach (Vector2 force in quadTreeForces)
            {
                if (!float.IsNaN(force.X) && !float.IsNaN(force.Y))
                {
                    acceleration += force / mass;
                }
            }
            foreach (Vector2 force in forces)
            {
                if (!float.IsNaN(force.X) && !float.IsNaN(force.Y))
                {
                    acceleration += force / mass;
                }
            }

            acceleration -= friction * velocity;
            velocity += acceleration;
            Position += velocity;
        }
    }
}

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
        
        private float mass;

        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        private float restitution;

        public float Restitution
        {
            get { return restitution; }
            set { restitution = value; }
        }

        public int SpatialGridId;

        public Vector2 Momentum { get { return velocity * mass; } }

        public Particle(Vector2 position, Vector2 velocity, float mass, float restitution)
        {
            this.position = position;
            this.velocity = velocity;
            this.mass = mass;
            this.restitution = restitution;
            forces = new List<Vector2>();
        }

        public virtual void Update(GameTime gameTime)
        {
            acceleration = Vector2.Zero;

            foreach (Vector2 force in forces)
            {
                acceleration += force / mass;
            }

            Position += velocity += acceleration;
        }
    }
}

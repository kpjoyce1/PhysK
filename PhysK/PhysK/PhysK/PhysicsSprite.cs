using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysK
{
    class PhysicsSprite : Sprite
    {
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

        private float angularVelocity;

        public float AngularVelocity
        {
            get { return angularVelocity; }
            set { angularVelocity = value; }
        }

        private float angularAcceleration;

        public float AngularAcceleration
        {
            get { return angularAcceleration; }
            set { angularAcceleration = value; }
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

        private Rectangle allignedHitbox;

        public Rectangle AllignedHitbox
        {
            get { return allignedHitbox; }
            set { allignedHitbox = value; }
        }


        private Vector2 center;

        public Vector2 Center
        {
            get { return center; }
            set { center = value; }
        }

        private float radius;

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public Vector2 Momentum { get { return velocity * mass; } }

        public int SpatialGridID;

        public PhysicsSprite(Texture2D texture, Vector2 position, Color color, Vector2 velocity, float mass, float restitution)
            : base(texture, position, texture.Bounds, color, 0f, Vector2.Zero, Vector2.One, 1f)
        {
            this.velocity = velocity;
            this.mass = mass;
            this.restitution = restitution;
            forces = new List<Vector2>();
            this.allignedHitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            this.radius = texture.Width / 2;
        }

        

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(World.pixel, allignedHitbox, null, Color.Red, rotation, origin, effects, layerDepth);
        }

        public void Update(GameTime gameTime)
        {
            allignedHitbox.X = (int)position.X;
            allignedHitbox.Y = (int)position.Y;

            center = position + new Vector2(texture.Width / 2, texture.Height / 2);

            acceleration = Vector2.Zero;

            foreach(Vector2 force in forces)
            {
                acceleration += force / mass;
            }

            velocity += acceleration;

            position += velocity;

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysK
{
    public class Rigidbody : Particle
    {
        private float rotation;

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
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

        private Shape shape;

        public Shape Shape => shape;

        public Rigidbody(Shape shape, Vector2 position, Vector2 velocity, float mass, float charge, float restitution)
            : base(position, velocity, mass, charge, restitution)
        {
            this.shape = shape;
            
        }

        public override void Update(GameTime gameTime)
        {
            angularAcceleration = 0;
            rotation += angularVelocity += angularAcceleration;
            base.Update(gameTime);
        }
    }
}

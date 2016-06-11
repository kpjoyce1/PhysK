using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysK
{
    public class DebugView
    {
        public Color HollowColor { get; set; }
        public Color FilledColor { get; set; }

        private DebugDrawBatcher batcher;

        private World world;

        public World World
        {
            get { return world; }
            set { world = value; }
        }

        public DebugView(GraphicsDevice graphicsDevice, World world)
        {
            batcher = new DebugDrawBatcher(graphicsDevice);
           
            World = world;

            HollowColor = Color.Green;

            FilledColor = Color.Red;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            batcher.Begin(view, projection);

            foreach (Particle particle in world.Items)
            {
                Color color = (particle is Rigidbody && (particle as Rigidbody).Shape.IsHollow) ? HollowColor : FilledColor;
                batcher.AddParticle(particle, color);
            }

            batcher.End();
        }
    }
}

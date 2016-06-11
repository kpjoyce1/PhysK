using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysK
{
    public class DebugView
    {
        private static VertexPositionColor[] circleVertexPositionColors;
        private static Dictionary<Shape, VertexPositionColor[]> vertexPositionColors;

        private readonly GraphicsDevice graphicsDevice;

        private readonly BasicEffect basicEffect;

        private World world;

        public World World
        {
            get { return world; }
            set { world = value; }
        }

        public DebugView(GraphicsDevice graphicsDevice, World world)
        {
            if (circleVertexPositionColors == null)
            {
                circleVertexPositionColors = new VertexPositionColor[102];
                circleVertexPositionColors[0] = new VertexPositionColor(Vector3.Zero, Color.White);
                for (int i = 1; i < circleVertexPositionColors.Length - 1; i++)
                {
                    circleVertexPositionColors[i] = new VertexPositionColor(MathUtils.GetUnitCircle(MathHelper.TwoPi * i / circleVertexPositionColors.Length).ToVector3(), Color.Red);
                }
                circleVertexPositionColors[circleVertexPositionColors.Length - 1] = circleVertexPositionColors[1];
                vertexPositionColors = new Dictionary<Shape, VertexPositionColor[]>();
            }
            this.graphicsDevice = graphicsDevice;
            basicEffect = new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = true
            };
            World = world;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            basicEffect.View = view;
            basicEffect.Projection = projection;

            foreach (Particle particle in world.Items)
            {
                if (particle is Rigidbody)
                {
                    if ((particle as Rigidbody).Shape is Circle)
                    {
                        basicEffect.World = Matrix.CreateScale((particle as Rigidbody).Shape.Aabb.Size.ToVector3() / 2) *
                                            Matrix.CreateFromYawPitchRoll(0, 0, (particle as Rigidbody).Rotation) *
                                            Matrix.CreateTranslation(particle.Position.ToVector3());
                        basicEffect.CurrentTechnique.Passes[0].Apply();
                        graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, circleVertexPositionColors, 0, circleVertexPositionColors.Length - 1);
                    }
                    else
                    {
                        if (!vertexPositionColors.ContainsKey((particle as Rigidbody).Shape))
                        {
                            VertexPositionColor[] vertices = new VertexPositionColor[(particle as Rigidbody).Shape.Vertices.Length];
                            for (int i = 0; i < (particle as Rigidbody).Shape.Vertices.Length; i++)
                            {
                                vertices[i] = new VertexPositionColor((particle as Rigidbody).Shape.Vertices[i].ToVector3(), Color.Red);
                            }
                        }
                        basicEffect.World = Matrix.CreateScale((particle as Rigidbody).Shape.Aabb.Size.ToVector3()) *
                                            Matrix.CreateFromYawPitchRoll(0, 0, (particle as Rigidbody).Rotation) *
                                            Matrix.CreateTranslation(particle.Position.ToVector3());
                        basicEffect.CurrentTechnique.Passes[0].Apply();
                        graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertexPositionColors[(particle as Rigidbody).Shape], 0, vertexPositionColors[(particle as Rigidbody).Shape].Length - 1);
                    }
                }
                else
                {
                    basicEffect.World = Matrix.CreateTranslation(particle.Position.ToVector3());
                    basicEffect.CurrentTechnique.Passes[0].Apply();
                    graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, circleVertexPositionColors, 0, circleVertexPositionColors.Length - 1);
                }
            }
        }
    }
}

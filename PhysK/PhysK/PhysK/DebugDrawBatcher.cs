using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysK
{
    internal class DebugDrawBatcher
    {
        private static Vector2[] circleVertexPositionColors;
        private static Dictionary<Shape, Vector2[]> vertexPositionColors;

        public readonly GraphicsDevice graphicsDevice;

        public readonly BasicEffect basicEffect;

        private List<VertexPositionColor> lineVertexPositionColors;
        private List<VertexPositionColor> triangleVertexPositionColors;

        private bool drawStarted;

        public DebugDrawBatcher(GraphicsDevice graphicsDevice)
        {
            if (circleVertexPositionColors == null)
            {
                circleVertexPositionColors = new Vector2[52];
                circleVertexPositionColors[0] = Vector2.Zero;
                for (int i = 1; i < circleVertexPositionColors.Length - 1; i++)
                {
                    circleVertexPositionColors[i] = MathUtils.GetUnitCircle(MathHelper.TwoPi * i / circleVertexPositionColors.Length);
                }
                circleVertexPositionColors[circleVertexPositionColors.Length - 1] = circleVertexPositionColors[1];
                vertexPositionColors = new Dictionary<Shape, Vector2[]>();
            }

            this.graphicsDevice = graphicsDevice;
            basicEffect = new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = true
            };

            lineVertexPositionColors = new List<VertexPositionColor>();
            triangleVertexPositionColors = new List<VertexPositionColor>();

            drawStarted = false;
        }

        public void Begin(Matrix view, Matrix projection)
        {
            lineVertexPositionColors.Clear();
            triangleVertexPositionColors.Clear();

            basicEffect.View = view;
            basicEffect.Projection = projection;

            basicEffect.CurrentTechnique.Passes[0].Apply();

            drawStarted = true;
        }

        public void AddParticle(Particle particle, Color color)
        {
            addPoint(particle.Position, color);
            
            if (particle is Rigidbody)
            {
                Rigidbody rigidbody = particle as Rigidbody;
                
                if (rigidbody.Shape is Circle)
                {
                    Matrix transform = Matrix.CreateScale(rigidbody.Shape.Aabb.Size.ToVector3() / 2) *
                                        Matrix.CreateFromYawPitchRoll(0, 0, (particle as Rigidbody).Rotation) *
                                        Matrix.CreateTranslation(particle.Position.ToVector3());
                    for (int i = 0; i < circleVertexPositionColors.Length - 1; i++)
                    {
                        addLine(Vector2.Transform(circleVertexPositionColors[i], transform),
                            Vector2.Transform(circleVertexPositionColors[i + 1], transform),
                            color);
                    }
                }
                else
                {
                    Matrix transform = Matrix.CreateScale(rigidbody.Shape.Aabb.Size.ToVector3()) *
                                        Matrix.CreateFromYawPitchRoll(0, 0, (particle as Rigidbody).Rotation) *
                                        Matrix.CreateTranslation(particle.Position.ToVector3());
                    for (int i = 0; i < rigidbody.Shape.Vertices.Length - 1; i++)
                    {
                        addLine(Vector2.Transform(circleVertexPositionColors[i], transform),
                            Vector2.Transform(circleVertexPositionColors[i + 1], transform),
                            color);
                    }
                }
            }
        }

        private void addPoint(Vector2 position, Color color)
        {
            Matrix transform = Matrix.CreateScale(Vector3.One * 5) * 
                                Matrix.CreateTranslation(position.ToVector3());
            for (int i = 0; i < circleVertexPositionColors.Length - 1; i++)
            {
                addTriangle(
                    position,
                    Vector2.Transform(circleVertexPositionColors[i], transform),
                    Vector2.Transform(circleVertexPositionColors[i + 1], transform),
                    color);
            }
        }

        private void addLine(Vector2 start, Vector2 end, Color color)
        {
            lineVertexPositionColors.AddRange(
                new VertexPositionColor[]
                {
                    new VertexPositionColor(start.ToVector3(), color),
                    new VertexPositionColor(end.ToVector3(), color)
                }
            );
        }

        private void addTriangle(Vector2 a, Vector2 b, Vector2 c, Color color)
        {
            triangleVertexPositionColors.AddRange(
                new VertexPositionColor[]
                {
                    new VertexPositionColor(a.ToVector3(), color),
                    new VertexPositionColor(b.ToVector3(), color), 
                    new VertexPositionColor(c.ToVector3(), color), 
                }
            );
        }

        public void End()
        {
            if (lineVertexPositionColors.Count > 1)
            {
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList,
                    lineVertexPositionColors.ToArray(),
                    0,
                    lineVertexPositionColors.Count / 2);
            }
            if (triangleVertexPositionColors.Count > 2)
            {
                graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                    triangleVertexPositionColors.ToArray(),
                    0,
                    triangleVertexPositionColors.Count / 3);
            }
            drawStarted = false;
        }
    }
}

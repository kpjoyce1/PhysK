using System.Linq;
using Microsoft.Xna.Framework;

namespace PhysK
{
    public class Shape
    {
        private Vector2[] vertices;

        public Vector2[] Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        private Rectangle aabb;

        public Rectangle AABB
        {
            get { return aabb; }
            set { aabb = value; }
        }

        public Shape(params Vector2[] vertices)
        {
            this.vertices = vertices;
            aabb = new Rectangle();
            if (vertices.Length > 0)
            {
                aabb.X = (int) vertices.ToList().Min(vertex => vertex.X);
                aabb.Y = (int) vertices.ToList().Min(vertex => vertex.Y);
                aabb.Width = (int) (vertices.ToList().Max(vertex => vertex.X) - aabb.X);
                aabb.Height = (int) (vertices.ToList().Max(vertex => vertex.Y) - aabb.Y);
            }
        }
    }
}
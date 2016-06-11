using System.Linq;
using Microsoft.Xna.Framework;

namespace PhysK
{
    public class Shape
    {
        // Used for Edge Colliders
        private bool isHollow;

        public bool IsHollow
        {
            get { return isHollow; }
            set { isHollow = value; }
        }

        private Vector2[] vertices;

        public Vector2[] Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        private RectangleF aabb;

        public RectangleF AABB
        {
            get { return aabb; }
            set { aabb = value; }
        }

        public Shape(params Vector2[] vertices)
        {
            this.vertices = vertices;
            aabb = new RectangleF();
            UpdateAABB();
        }

        public void UpdateAABB()
        {
            if (vertices.Length > 0)
            {
                aabb.X = vertices.ToList().Min(vertex => vertex.X);
                aabb.Y = vertices.ToList().Min(vertex => vertex.Y);
                aabb.Width = vertices.ToList().Max(vertex => vertex.X) - aabb.X;
                aabb.Height = vertices.ToList().Max(vertex => vertex.Y) - aabb.Y;
            }
        }
    }
}
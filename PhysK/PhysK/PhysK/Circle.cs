using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysK
{
    public class Circle : Shape
    {
        private float radius;

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public Circle(float radius)
        {
            this.radius = radius;
            Aabb = new RectangleF(-radius, -radius, radius * 2, radius * 2);
        }
    }
}

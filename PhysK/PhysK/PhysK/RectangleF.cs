using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhysK
{
    public struct RectangleF
    {
        private float x;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        private float y;

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        private float width;

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        private float height;

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public float Left
        {
            get { return x; }
        }

        public float Right
        {
            get { return x + width; }
        }

        public float Top
        {
            get { return y; }
        }

        public float Bottom
        {
            get { return y + height; }
        }

        public RectangleF(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = x;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Checks to see if rectangle is fully contained within this rectangle
        /// </summary>
        /// <param name="rectangleF"></param>
        /// <returns></returns>
        public bool Contains(RectangleF rectangleF)
        {
            return !isLeft(rectangleF.Right) && 
                   !isRight(rectangleF.Left) && 
                   !isBottom(rectangleF.Top) && 
                   !isTop(rectangleF.Bottom);
        }

        public bool Intersects(RectangleF rectangleF)
        {
            return (!isLeft(rectangleF.Right)   || !isRight(rectangleF.Left) &&
                   (!isTop(rectangleF.Top)      || !isBottom(rectangleF.Bottom)));
        }

        public bool Contains(Vector2 point)
        {
            return !isLeft(point.X) &&
                   !isRight(point.X) &&
                   !isBottom(point.Y) &&
                   !isTop(point.Y);
        }

        public bool isLeft(float x)
        {
            return x < Left;
        }

        public bool isRight(float x)
        {
            return x > Right;
        }

        public bool isTop(float y)
        {
            return y < Top;
        }

        public bool isBottom(float y)
        {
            return y > Bottom;
        }

    }
}

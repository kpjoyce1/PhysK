using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhysK
{
    public struct RectangleF
    {
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        private Vector2 size;

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public float Width
        {
            get { return size.X; }
            set { size.Y = value; }
        }

        public float Height
        {
            get { return size.Y; }
            set { size.Y = value; }
        }

        public float Left => position.X;

        public float Right => position.X + size.X;

        public float Top => position.Y;

        public float Bottom => position.Y + size.Y;

        public RectangleF(float x, float y, float width, float height)
            : this(new Vector2(x, y), new Vector2(width, height))
        { }

        public RectangleF(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }

        public void Offset(Vector2 amount)
        {
            Position += amount;
        }

        public void Inflate(Vector2 amount)
        {
            Size += amount;
        }

        /// <summary>
        /// Checks to see if rectangle is fully contained within this rectangle
        /// </summary>
        /// <param name="rectangleF"></param>
        /// <returns></returns>
        public bool Contains(RectangleF rectangleF)
        {
            return !IsLeft(rectangleF.Right) && 
                   !IsRight(rectangleF.Left) && 
                   !IsBottom(rectangleF.Top) && 
                   !IsTop(rectangleF.Bottom);
        }

        public bool Intersects(RectangleF rectangleF)
        {
            return (!IsLeft(rectangleF.Right)   || !IsRight(rectangleF.Left) &&
                   (!IsTop(rectangleF.Top)      || !IsBottom(rectangleF.Bottom)));
        }

        public bool Contains(Vector2 point)
        {
            return !IsLeft(point.X) &&
                   !IsRight(point.X) &&
                   !IsBottom(point.Y) &&
                   !IsTop(point.Y);
        }

        public bool IsLeft(float x)
        {
            return x < Left;
        }

        public bool IsRight(float x)
        {
            return x > Right;
        }

        public bool IsTop(float y)
        {
            return y < Top;
        }

        public bool IsBottom(float y)
        {
            return y > Bottom;
        }

    }
}

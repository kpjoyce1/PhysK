using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysK
{
    public static class MathUtils
    {
        public static Vector2 GetUnitCircle(float rotation)
        {
            return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
        }

        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.X, vector3.Y);
        }
        
        public static Vector3 ToVector3(this Vector2 vector2, float z = 0)
        {
            return new Vector3(vector2.X, vector2.Y, z);
        }

        public static Vector2 GetSize(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Width, rectangle.Height);
        }
    }
}

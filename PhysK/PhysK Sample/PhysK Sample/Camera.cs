using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysK;

namespace PhysKSample
{
    public class Camera
    {
        private static Camera main;

        public static Camera Main
        {
            get { return main; }
            private set { main = main??value; }
        }

        public GraphicsDevice GraphicsDevice { get; private set; }

        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                UpdateView();
            }
        }

        private float rotation;

        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value; 
                UpdateView();
            }
        }

        public Matrix View { get; private set; }
        
        private Vector2 scale;

        public Vector2 Scale
        {
            get { return scale; }
            set
            {
                scale = value; 
                UpdateProjection();
            }
        }

        public Matrix Projection { get; private set; }

        public Camera(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            position = Vector2.Zero;
            rotation = 0f;
            scale = Vector2.One;
            UpdateView();
            UpdateProjection();
        }

        private void UpdateView()
        {
            View = Matrix.CreateLookAt(position.ToVector3(-1), position.ToVector3(), MathUtils.GetUnitCircle(rotation - MathHelper.PiOver2).ToVector3());
        }

        private void UpdateProjection()
        {
            Projection = Matrix.CreateOrthographic(GraphicsDevice.Viewport.Width * scale.X, GraphicsDevice.Viewport.Height * scale.Y, 0, 10);
        }
    }
}

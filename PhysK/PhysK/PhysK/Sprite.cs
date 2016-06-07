using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysK
{
    class Sprite
    {
        protected Texture2D texture;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        protected Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected Rectangle sourceRectangle;

        public Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        protected Color color;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        protected float rotation;

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        protected Vector2 origin;

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        protected Vector2 scale;

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        protected SpriteEffects effects;

        public SpriteEffects Effects
        {
            get { return effects; }
            set { effects = value; }
        }

        protected float layerDepth;

        public float LayerDepth
        {
            get { return layerDepth; }
            set { layerDepth = value; }
        }


        public Sprite(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, float layerDepth)
        {
            this.texture = texture;
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.layerDepth = layerDepth;
        }

        public Sprite(Texture2D texture, Vector2 position, Color color)
            : this(texture, position, Rectangle.Empty, color, 0f, Vector2.Zero, Vector2.One, 1f)
        { }
        

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

    }
}

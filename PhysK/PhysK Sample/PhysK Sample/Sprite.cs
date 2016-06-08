using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysKSample
{
    public class Sprite
    {
        #region Variables
        private static Texture2D pixel;

        private static Texture2D Pixel
        {
            get
            {
                if (pixel != null) return pixel;

                pixel = new Texture2D(Camera.Main.GraphicsDevice, 1, 1);
                pixel.SetData<Color>(new Color[] { Color.White });
                return pixel;
            }
        }

        private Texture2D texture;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private Rectangle sourceRectangle;

        public Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        private Color color;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private float rotation;

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        private Vector2 origin;

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        private Vector2 scale;

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private SpriteEffects effects;

        public SpriteEffects Effects
        {
            get { return effects; }
            set { effects = value; }
        }

        private float layerDepth;

        public float LayerDepth
        {
            get { return layerDepth; }
            set { layerDepth = value; }
        }
        #endregion

        #region Constructors
        public Sprite()
            : this(null)
        { }
        public Sprite(Texture2D texture)
            : this(texture, Vector2.Zero)
        { }
        public Sprite(Texture2D texture, Vector2 position)
            : this(texture, position, Color.White)
        { }
        public Sprite(Texture2D texture, Vector2 position, Color color)
            : this(texture, position, color, Vector2.Zero)
        { }
        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 origin)
            : this(texture, position, color, origin, 0f)
        { }
        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 origin, float rotation)
            : this(texture, position, color, origin, rotation, 1f)
        { }
        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 origin, float rotation, float scale)
            : this(texture, position, color, origin, rotation, Vector2.One * scale)
        { }
        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 origin, float rotation, Vector2 scale)
            : this(texture, position, color, origin, rotation, scale, SpriteEffects.None)
        { }
        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 origin, float rotation, float scale, SpriteEffects effects)
            : this(texture, position, color, origin, rotation, Vector2.One * scale, effects)
        { }
        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 origin, float rotation, Vector2 scale, SpriteEffects effects)
            : this(texture, position, color, origin, rotation, scale, effects, 0f)
        { }
        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 origin, float rotation, float scale, SpriteEffects effects, float layerDepth)
            : this(texture, position, color, origin, rotation, Vector2.One * scale, effects, layerDepth)
        { }
        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 origin, float rotation, Vector2 scale, SpriteEffects effects, float layerDepth)
            : this(texture, position, texture.Bounds, color, origin, rotation, scale, effects, layerDepth)
        { }

        public Sprite(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, Vector2 origin, float rotation, float scale, SpriteEffects effects, float layerDepth)
            : this(texture, position, sourceRectangle, color, origin, rotation, Vector2.One * scale, effects, layerDepth)
        { }
        #endregion

        public Sprite(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, Vector2 origin, float rotation, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            this.texture = texture??pixel;
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.layerDepth = layerDepth;
        }

        public virtual void SetCenterOrigin()
        {
            origin = new Vector2(sourceRectangle.Width, sourceRectangle.Height) / 2;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

    }
}

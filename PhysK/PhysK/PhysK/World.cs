using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysK
{
    class World
    {
        public static Texture2D pixel;

        private PhysicsSprite [] items;
        
        public PhysicsSprite [] Items
        {
            get { return items; }
            set { items = value; }
        }

        private Rectangle bounds;

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        private Rectangle[] grid;

        public World(GraphicsDevice graphicsDevice)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            items = new PhysicsSprite [100];

            bounds = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

            grid = new Rectangle[4];

            grid[0] = new Rectangle(0, 0, graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);

            grid[1] = new Rectangle(graphicsDevice.Viewport.Width / 2, 0, graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);

            grid[2] = new Rectangle(0, graphicsDevice.Viewport.Height / 2, graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);

            grid[3] = new Rectangle(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2, graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);

        }

        public void Update(GameTime gameTime)
        {
            foreach(PhysicsSprite item in items)
            {
                item.Update(gameTime);
            }

            for(int i = 0; i < items.Length; i++)
            {
                items[i].Update(gameTime);
                
                if (items[i].AllignedHitbox.Left < bounds.Left)
                {
                    items[i].Position += new Vector2(-items[i].AllignedHitbox.Left, 0);
                    items[i].Velocity = new Vector2(-items[i].Velocity.X, items[i].Velocity.Y);
                }
                else if (items[i].AllignedHitbox.Right > bounds.Right)
                {
                    items[i].Position -= new Vector2(items[i].AllignedHitbox.Right - bounds.Right, 0);
                    items[i].Velocity = new Vector2(-items[i].Velocity.X, items[i].Velocity.Y);
                }

                if (items[i].AllignedHitbox.Top < bounds.Top)
                {
                    items[i].Position += new Vector2(0, -items[i].AllignedHitbox.Top);
                    items[i].Velocity = new Vector2(items[i].Velocity.X, -items[i].Velocity.Y);
                }
                else if (items[i].AllignedHitbox.Bottom > bounds.Bottom)
                {
                    items[i].Position -= new Vector2(0, items[i].AllignedHitbox.Bottom - bounds.Bottom);
                    items[i].Velocity = new Vector2(items[i].Velocity.X, -items[i].Velocity.Y);
                }

                for (int j = i + 1; j < items.Length; j++)
                {

                    if (Vector2.Distance(items[i].Center + items[i].Velocity, items[j].Center + items[j].Velocity) < items[i].Radius + items[j].Radius)
                    { 
                        //collission
                        Vector2 initialI = items[i].Velocity;
                        Vector2 initialJ = items[j].Velocity;

                        Vector2 normal = items[i].Center - items[j].Center;
                        normal.Normalize();

                        Vector2 effectiveVelocityI = Vector2.Dot(initialI, normal) * normal;
                        Vector2 effectiveVelocityJ = Vector2.Dot(initialJ, normal) * normal;

                        Vector2 unchangedVeloctiyI = initialI - effectiveVelocityI;
                        Vector2 unchangedVelocityJ = initialJ - effectiveVelocityJ;

                        
                        Vector2 changedVelocityI = (effectiveVelocityI * (items[i].Mass - items[j].Mass) + 2 * items[j].Mass * effectiveVelocityJ) / (items[i].Mass + items[j].Mass);
                        Vector2 changedVelocityJ = (effectiveVelocityJ * (items[j].Mass - items[i].Mass) + 2 * items[i].Mass * effectiveVelocityI) / (items[i].Mass + items[j].Mass);

                        items[i].Velocity = unchangedVeloctiyI + changedVelocityI;
                        items[j].Velocity = unchangedVelocityJ + changedVelocityJ;
                    }
                }
                
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float Hamiltonian = 0;
            foreach(Sprite sprite in Items)
            {
                sprite.Draw(spriteBatch);
                if(sprite is PhysicsSprite)
                {
                    Hamiltonian += ((PhysicsSprite)sprite).Velocity.LengthSquared() * ((PhysicsSprite)sprite).Mass;
                }
                
            }
            spriteBatch.DrawString(GameApplication.font, Hamiltonian.ToString(), Vector2.Zero, Color.Black);
        }
    }
}

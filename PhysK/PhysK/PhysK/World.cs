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
        public static Texture2D pixel; /*!< A static pixel for debugging purposes */

        private PhysicsSprite[] items; /*!< The physics objects contained in the world that will be interacting */

        public PhysicsSprite[] Items
        {
            get { return items; }
            set { items = value; }
        }

        private Rectangle bounds; /*!<The bounds of the world */

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }


        private SpatialGrid[] grids;

        public bool Permeable; /*!< Bool for if items are allowed outside the world bounds*/

        public World(GraphicsDevice graphicsDevice, bool permeable = false)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            items = new PhysicsSprite[100];

            bounds = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

            grids = new SpatialGrid[4];

            grids[0] = new SpatialGrid(new Rectangle(0, 0, graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2));

            grids[1] = new SpatialGrid(new Rectangle(graphicsDevice.Viewport.Width / 2, 0, graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2));

            grids[2] = new SpatialGrid(new Rectangle(0, graphicsDevice.Viewport.Height / 2, graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2));

            grids[3] = new SpatialGrid(new Rectangle(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2, graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2));

            this.Permeable = permeable;
        }

        public void Update(GameTime gameTime)
        {
            for(int i = 0; i < items.Length; i++)
            {
                items[i].Update(gameTime);
                WorldContainment(items[i]);
            }



            for (int i = 0; i < items.Length; i++)
            {
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

        private void WorldContainment(PhysicsSprite entity)
        {
            if (!Permeable)
            {
                if (entity.AllignedHitbox.Left < bounds.Left)
                {
                    entity.Velocity = new Vector2(Math.Abs(entity.Velocity.X), entity.Velocity.Y);
                }
                else if (entity.AllignedHitbox.Right > bounds.Right)
                {
                    entity.Velocity = new Vector2(-Math.Abs(entity.Velocity.X), entity.Velocity.Y);
                }

                if (entity.AllignedHitbox.Top < bounds.Top)
                {
                    entity.Velocity = new Vector2(entity.Velocity.X, Math.Abs(entity.Velocity.Y));
                }
                else if (entity.AllignedHitbox.Bottom > bounds.Bottom)
                {
                    entity.Velocity = new Vector2(entity.Velocity.X, -Math.Abs(entity.Velocity.Y));
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

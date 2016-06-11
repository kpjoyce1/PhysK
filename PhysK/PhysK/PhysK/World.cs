using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysK
{
    public class World
    {
        public static GraphicsDevice GraphicsDevice { private set; get; }

        private Particle[] items; /*!< The physics objects contained in the world that will be interacting */

        public Particle[] Items
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

        public float Hamiltonian { get; private set; }

        private SpatialGrid[] grids;

        public bool Permeable; /*!< Bool for if items are allowed outside the world bounds*/

        public World(GraphicsDevice graphicsDevice, bool permeable = false)
        {
            GraphicsDevice = graphicsDevice;

            items = new Particle[100];

            bounds = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

            grids = new SpatialGrid[4];

            grids[0] = new SpatialGrid(new RectangleF(0, 0, graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f));

            grids[1] = new SpatialGrid(new RectangleF(graphicsDevice.Viewport.Width / 2f, 0, graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f));

            grids[2] = new SpatialGrid(new RectangleF(0, graphicsDevice.Viewport.Height / 2f, graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f));

            grids[3] = new SpatialGrid(new RectangleF(graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f, graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f));

            this.Permeable = permeable;
        }

        public void Update(GameTime gameTime)
        {
            Hamiltonian = 0;
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Update(gameTime);
                if (items[i] is Rigidbody)
                {
                    WorldContainment(items[i] as Rigidbody);
                }
                else
                {
                    WorldContainment(items[i]);
                }
                Hamiltonian += items[i].Velocity.LengthSquared() * items[i].Mass;
            }

            for (int i = 0; i < items.Length; i++)
            {
                for (int j = i + 1; j < items.Length; j++)
                {
                    if (Vector2.Distance(items[i].Position + items[i].Velocity, items[j].Position + items[j].Velocity) < 
                                                                                                    (items[i] is Rigidbody ? (items[i] as Rigidbody).Shape.AABB.Width / 2 : 0) +
                                                                                                    (items[j] is Rigidbody ? (items[j] as Rigidbody).Shape.AABB.Width / 2: 0))
                    {
                        // collision
                        Vector2 initialI = items[i].Velocity;
                        Vector2 initialJ = items[j].Velocity;

                        Vector2 normal = items[i].Position - items[j].Position;
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

        private void WorldContainment(Particle entity)
        {
            if (!Permeable)
            {
                if (entity.Position.X < bounds.Left)
                {
                    entity.Velocity = new Vector2(Math.Abs(entity.Velocity.X), entity.Velocity.Y);
                }
                else if (entity.Position.X > bounds.Right)
                {
                    entity.Velocity = new Vector2(-Math.Abs(entity.Velocity.X), entity.Velocity.Y);
                }

                if (entity.Position.Y < bounds.Top)
                {
                    entity.Velocity = new Vector2(entity.Velocity.X, Math.Abs(entity.Velocity.Y));
                }
                else if (entity.Position.Y > bounds.Bottom)
                {
                    entity.Velocity = new Vector2(entity.Velocity.X, -Math.Abs(entity.Velocity.Y));
                }

            }
        }

        private void WorldContainment(Rigidbody entity)
        {
            if (!Permeable)
            {
                if (entity.Shape.AABB.Left + entity.Position.X < bounds.Left)
                {
                    entity.Velocity = new Vector2(Math.Abs(entity.Velocity.X), entity.Velocity.Y);
                }
                else if (entity.Shape.AABB.Right + entity.Position.X > bounds.Right)
                {
                    entity.Velocity = new Vector2(-Math.Abs(entity.Velocity.X), entity.Velocity.Y);
                }

                if (entity.Shape.AABB.Top + entity.Position.Y < bounds.Top)
                {
                    entity.Velocity = new Vector2(entity.Velocity.X, Math.Abs(entity.Velocity.Y));
                }
                else if (entity.Shape.AABB.Bottom + entity.Position.Y > bounds.Bottom)
                {
                    entity.Velocity = new Vector2(entity.Velocity.X, -Math.Abs(entity.Velocity.Y));
                }

            }
        }
    }
}

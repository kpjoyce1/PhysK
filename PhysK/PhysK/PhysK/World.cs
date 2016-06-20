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

        private RectangleF bounds; /*!<The bounds of the world */

        public RectangleF Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        private QuadTree quadTree;

        public float Hamiltonian { get; private set; }

        public bool Permeable; /*!< Bool for if items are allowed outside the world bounds*/

        private List<Particle> collideables = new List<Particle>();

        public World(GraphicsDevice graphicsDevice, bool permeable = false)
        {
            GraphicsDevice = graphicsDevice;

            items = new Particle[100];
                
            bounds = new RectangleF(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

            quadTree = new QuadTree(0, bounds);
            QuadTree.Pixel = new Texture2D(graphicsDevice, 1, 1);
            this.Permeable = permeable;
        }

        public void Update(GameTime gameTime)
        {
            Hamiltonian = 0;
            quadTree.Clear();
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Update(gameTime);
                quadTree.Insert(items[i]);
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
                collideables.Clear();

                quadTree.Retrieve(collideables, items[i]);

                for (int j = 0; j < collideables.Count; j++)
                {

                    if (collideables[j] != items[i] && Vector2.Distance(items[i].Position + items[i].Velocity, collideables[j].Position + collideables[j].Velocity) <
                                                                                                    (items[i] is Rigidbody ? (items[i] as Rigidbody).Shape.AABB.Width / 2 : float.Epsilon) +
                                                                                                    (collideables[j] is Rigidbody ? (collideables[j] as Rigidbody).Shape.AABB.Width / 2 : float.Epsilon))
                    {
                        // collision

                        Vector2 initialI = items[i].Velocity;
                        Vector2 initialJ = collideables[j].Velocity;

                        Vector2 normal = items[i].Position - collideables[j].Position;
                        normal.Normalize();

                        Vector2 effectiveVelocityI = Vector2.Dot(initialI, normal) * normal;
                        Vector2 effectiveVelocityJ = Vector2.Dot(initialJ, normal) * normal;

                        Vector2 unchangedVeloctiyI = initialI - effectiveVelocityI;
                        Vector2 unchangedVelocityJ = initialJ - effectiveVelocityJ;


                        Vector2 changedVelocityI = (effectiveVelocityI * (items[i].Mass - collideables[j].Mass) + 2 * collideables[j].Mass * effectiveVelocityJ) / (items[i].Mass + collideables[j].Mass);
                        Vector2 changedVelocityJ = (effectiveVelocityJ * (collideables[j].Mass - items[i].Mass) + 2 * items[i].Mass * effectiveVelocityI) / (items[i].Mass + collideables[j].Mass);

                        items[i].Velocity = unchangedVeloctiyI + changedVelocityI;
                        collideables[j].Velocity = unchangedVelocityJ + changedVelocityJ;

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

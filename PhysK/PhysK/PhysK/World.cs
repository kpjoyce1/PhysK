using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Threading.Tasks;

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
        public float UpdateTime
        {
            get; private set;
        }

        public bool Permeable; /*!< Bool for if items are allowed outside the world bounds*/


        public static float GravitationalConstant => 6.67498f * (float)Math.Pow(10, -11);

        public static float CoulumbConstant => 8.99f * (float)Math.Pow(10, 0); //10^9


        public World(GraphicsDevice graphicsDevice, bool permeable = false)
        {
            GraphicsDevice = graphicsDevice;

            items = new Particle[100];
            watch = new Stopwatch();
            bounds = new RectangleF(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

            quadTree = new QuadTree(0, bounds);
            QuadTree.Pixel = new Texture2D(graphicsDevice, 1, 1);
            this.Permeable = permeable;
        }

        private Stopwatch watch;

        public void Update(GameTime gameTime)
        {

            watch.Start();
            Hamiltonian = 0;
            quadTree.Mass = 0;
            quadTree.Clear();
            quadTree.Mass = 0f;
            quadTree.MassPosition = Vector2.Zero;
            quadTree.Charge = 0f;
            quadTree.ChargePosition = Vector2.Zero;

            for (int i = 0; i < items.Length; i++)
            {
                quadTree.Insert(items[i]);
            }
            ;
            Parallel.ForEach(items, (item) =>
            {
                item.QuadTreeForces.Clear();
                
                item.QuadTreeForces.Add(quadTree.GetEffectiveGravity(Vector2.Zero, item));
                item.QuadTreeForces.Add(quadTree.GetEffectiveCoulumbForce(Vector2.Zero, item));
                item.Update(gameTime);


                if (item is Rigidbody)
                {
                    WorldContainment(item as Rigidbody);
                }
                else
                {
                    WorldContainment(item);
                }

                if (item.Velocity == Vector2.Zero)
                {
                    item.FramesAtRest++;
                }
                else
                {
                    item.FramesAtRest = 0;
                }

                if (item.FramesAtRest > 3)
                {
                    item.Sleep = true;
                }
                else
                {
                    item.Sleep = false;
                }

                Hamiltonian += item.Velocity.LengthSquared() * item.Mass;


                if (!item.Sleep)
                {
                    List<Particle> collideables = new List<Particle>();
                    
                    quadTree.Retrieve(collideables, item);
                    
                    for (int j = 0; j < collideables.Count; j++)
                    {
                        if (collideables[j] != null && collideables[j] != item && Vector2.Distance(item.Position + item.Velocity, collideables[j].Position + collideables[j].Velocity) <
                                                                                                        (item is Rigidbody ? (item as Rigidbody).Shape.AABB.Width / 2 : float.Epsilon) +
                                                                                                        (collideables[j] is Rigidbody ? (collideables[j] as Rigidbody).Shape.AABB.Width / 2 : float.Epsilon))
                        {
                            // collision

                            Vector2 initialI = item.Velocity;
                            Vector2 initialJ = collideables[j].Velocity;

                            Vector2 normal = item.Position - collideables[j].Position;

                            

                            if (normal != Vector2.Zero)
                            {
                                normal.Normalize();
                            }
                            Vector2 effectiveVelocityI = Vector2.Dot(initialI, normal) * normal;
                            Vector2 effectiveVelocityJ = Vector2.Dot(initialJ, normal) * normal;

                            Vector2 unchangedVeloctiyI = initialI - effectiveVelocityI;
                            Vector2 unchangedVelocityJ = initialJ - effectiveVelocityJ;


                            Vector2 changedVelocityI = (effectiveVelocityI * (item.Mass - collideables[j].Mass) + 2 * collideables[j].Mass * effectiveVelocityJ) / (item.Mass + collideables[j].Mass);
                            Vector2 changedVelocityJ = (effectiveVelocityJ * (collideables[j].Mass - item.Mass) + 2 * item.Mass * effectiveVelocityI) / (item.Mass + collideables[j].Mass);

                            item.Velocity = unchangedVeloctiyI + changedVelocityI;
                            collideables[j].Velocity = unchangedVelocityJ + changedVelocityJ;

                            item.Position += item.Velocity;
                            collideables[j].Position += collideables[j].Velocity;

                        }

                    }
                }

            });
            
            watch.Stop();
            UpdateTime = watch.ElapsedMilliseconds;

            watch.Reset();

        }

        private void WorldContainment(Particle entity)
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

        private void WorldContainment(Rigidbody entity)
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

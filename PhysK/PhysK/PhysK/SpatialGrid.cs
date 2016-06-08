using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysK
{
    public class SpatialGrid
    {
        private Rectangle bounds; /*!< Spatail grid bounds */

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        //Consider using an array
        private List<Particle> items; /*!< Items contained within this grid */

        public List<Particle> Items
        {
            get { return items; }
            set { items = value; }
        }

        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }


        public SpatialGrid(Rectangle bounds)
        {
            this.bounds = bounds;
            this.items = new List<Particle>();
        }

        public void initiallyPopulate(Particle[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                AddItem(entities[i]);
            }
        }

        public void ItemsCheck()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] is Rigidbody)
                {
                    if (!(items[i] as Rigidbody).Shape.AABB.Intersects(this.bounds))
                    {
                        items.RemoveAt(i);
                    }
                }
                else
                {
                    if (!this.bounds.Contains((int)items[i].Position.X, (int)items[i].Position.Y))
                    {
                        items.RemoveAt(i);
                    }
                }
            }
        }

        public void AddItem(Particle entity)
        {
            if (entity is Rigidbody)
            {
                if (!(entity as Rigidbody).Shape.AABB.Intersects(this.bounds))
                {
                    items.Add(entity);
                }
            }
            else
            {
                if (!this.bounds.Contains((int)entity.Position.X, (int)entity.Position.Y))
                {
                    items.Add(entity);
                }
            }

        }

        public Particle[] getPossibleCollisions()
        {
            return items.ToArray();
        }

    }
}

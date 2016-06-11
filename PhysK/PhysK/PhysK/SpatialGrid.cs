using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysK
{
    public class SpatialGrid
    {
        private RectangleF bounds; /*!< Spatail grid bounds */

        public RectangleF Bounds
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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }


        public SpatialGrid(RectangleF bounds)
        {
            this.bounds = bounds;
            this.items = new List<Particle>();
        }

        public void InitiallyPopulate(Particle[] entities)
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
                    if (!(items[i] as Rigidbody).Shape.AABB.Intersects(bounds))
                    {
                        items.RemoveAt(i);
                    }
                }
                else
                {
                    if (!bounds.Contains(items[i].Position))
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
                if (!(entity as Rigidbody).Shape.AABB.Intersects(bounds))
                {
                    items.Add(entity);
                }
            }
            else
            {
                if (!this.bounds.Contains(entity.Position))
                {
                    items.Add(entity);
                }
            }

        }

        public Particle[] GetPossibleCollisions()
        {
            return items.ToArray();
        }

    }
}

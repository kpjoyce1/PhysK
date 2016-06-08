using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysK
{
    class SpatialGrid
    {
        private Rectangle bounds; /*!< Spatail grid bounds */

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        //Consider using an array
        private List<PhysicsSprite> items; /*!< Items contained within this grid */

        public List<PhysicsSprite> Items
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
            this.items = new List<PhysicsSprite>();
        }

        public void initiallyPopulate(PhysicsSprite [] entities)
        {
            for(int i = 0; i < entities.Length; i++)
            {
                if(entities[i].AllignedHitbox.Intersects(this.bounds))
                {
                    items.Add(entities[i]);
                }
            }
        }

        public void ItemsCheck()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if(!items[i].AllignedHitbox.Intersects(this.bounds))
                {
                    items.RemoveAt(i);
                }
            }
        }

        public void AddItem(PhysicsSprite entity)
        {
            if (!items.Contains(entity) && entity.AllignedHitbox.Intersects(this.bounds))
            {
                items.Add(entity);
                entity.SpatialGridID = this.id;
            }
        }

        public PhysicsSprite[] getPossibleCollisions()
        {
            return items.ToArray();
        }

    }
}

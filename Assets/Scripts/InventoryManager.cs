using System;
using System.Collections.Generic;
using System.Linq;

namespace GameDevFall2016.Scripts.InventoryManagement
{
    [System.Serializable]
    public class Inventory
    {
        private int _maxItemCount = 12;
        public int MaxItemCount
        {
            get
            {
                return _maxItemCount;
            }
        }

        public int CurrentItemCount
        {
            get
            {
                return Items.Count;
            }
        }

        public bool IsFull
        {
            get
            {
                return CurrentItemCount >= _maxItemCount;
            }
        }

        public bool ContainsItemWithId(int id)
        {
            return Items.Select(i => i.Id).Contains(id);
        }

        public List<InventoryItem> Items;
        public Inventory()
        {
            Items = new List<InventoryItem>(0);
        }

        public bool AddItemToInventory(InventoryItem itemToAdd)
        {
            bool containsId = ContainsItemWithId(itemToAdd.Id);

            if (!IsFull || containsId)
            {
                if(containsId)
                {
                    // Stack items with same id together
                    Items.FirstOrDefault(i => i.Id == itemToAdd.Id).Quantity += itemToAdd.Quantity;
                    return true;
                }
                else
                {
                    // Take up a new inventory slot
                    Items.Add(itemToAdd);
                    return false;
                }
            }
            else
            {
                // No room and nothing stacks
                return false;
            }
        }

        public void RemoveItem(int id, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);

            // Not found
            if (item == null) return;

            // remove quantity
            item.Quantity -= quantity;

            // Remove if all are gone
            if (item.Quantity <= 0)
            {
                Items.Remove(item);
            }
        }
    }
}

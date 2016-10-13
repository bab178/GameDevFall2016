using System.Collections.Generic;

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

        public List<InventoryItem> Items;
        public Inventory()
        {
            Items = new List<InventoryItem>(0);
        }

        public bool AddItemToInventory(InventoryItem itemToAdd)
        {
            if (CurrentItemCount < _maxItemCount)
            {
                Items.Add(itemToAdd);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

using System.Collections.Generic;

namespace GameDevFall2016.Scripts.InventoryManagement
{
    [System.Serializable]
    public class Inventory
    {
        private int MaxItems = 10;
        public List<Item> Items;

        Inventory()
        {
            Items = new List<Item>(0);
        }

        public bool AddItemToInventory(string name, int quantity)
        {
            if (Items.Count < MaxItems)
            {
                Items.Add(new Item(name, quantity));
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    [System.Serializable]
    public class Item
    {
        private string _name;
        private int _quantity;

        public Item(string name, int quantity)
        {
            _name = name;
            _quantity = quantity;
        }
    }
}

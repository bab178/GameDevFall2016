using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Models
{
    [System.Serializable]
    public class Inventory
    {
        #region Private Properties

        private const int _equipmentTypeSlots = 5;
        private const int _maxItemCount = 12;

        #endregion

        #region Public Properties

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

        public List<InventoryItem> Items { get; private set; }
        public InventoryItem[] Equipment { get; private set; }

        #endregion

        #region Constructors

        public Inventory()
        {
            Items = new List<InventoryItem>(0);
            Equipment = new InventoryItem[_equipmentTypeSlots];
        }

        #endregion

        #region Public Methods

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

        public void RemoveItem(InventoryItem itemToRemove)
        {
            var item = Items.FirstOrDefault(i => i.Equals(itemToRemove));

            // Not found
            if (item == null) return;

            Items.Remove(item);
        }

        public void EquipItemAtSlot(Inventory inv, InventoryItem itemToEquip)
        {
            // Not equipment / cannot equip
            if (itemToEquip.EquipType != EquipType.None) return;

            var slot = Convert.ToInt32(itemToEquip.EquipType);

            // Nothing worn, Equip to slot
            if (Equipment[slot] == null)
            {
                Equipment[slot] = itemToEquip;
                inv.RemoveItem(itemToEquip);
            }
            // Need to swap equipment
            else
            {
                // Swap items
                var itemToUnequip = Equipment[slot];
                Equipment[slot] = itemToEquip;
                inv.RemoveItem(itemToEquip);

                // put the unequipped item back in the inventory
                inv.AddItemToInventory(itemToUnequip);
            }
        }

        #endregion
    }
}

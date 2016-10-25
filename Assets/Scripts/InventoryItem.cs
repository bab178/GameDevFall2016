using UnityEngine;

namespace GameDevFall2016.Scripts.InventoryManagement
{
    [System.Serializable]
    public class InventoryItem : MonoBehaviour
    {
        public int Id;
        public Sprite Sprite;
        public string Name;
        public int Quantity;
        public string FlavorText;

        public InventoryItem(int id, Sprite sprite, string name, int quantity, string flavorText)
        {
            Id = id;
            Sprite = sprite;
            Name = name;
            Quantity = quantity;
            FlavorText = flavorText;
        }

        public InventoryItem()
        {
        }
    }
}
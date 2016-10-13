using UnityEngine;

namespace GameDevFall2016.Scripts.InventoryManagement
{
    [System.Serializable]
    public class InventoryItem : MonoBehaviour
    {
        public Sprite Sprite;
        public string Name;
        public int Quantity;
        public string FlavorText;

        public InventoryItem(Sprite sprite, string name, int quantity, string flavorText)
        {
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
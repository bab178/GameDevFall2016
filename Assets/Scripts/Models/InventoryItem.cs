using UnityEngine;

namespace Assets.Scripts.Models
{
    [System.Serializable]
    public enum EquipType { None, Head, Top, Bottom, Hands, Feet };

    [System.Serializable]
    public class InventoryItem : MonoBehaviour
    {
        public int Id;
        public Sprite Sprite;
        public string Name;
        public int Quantity;
        public string FlavorText;
        public EquipType EquipType;

        public Stats ItemStats { get; set; }

        public InventoryItem(int id, Sprite sprite, string name, int quantity, string flavorText, EquipType type = EquipType.None)
        {
            Id = id;
            Sprite = sprite;
            Name = name;
            Quantity = quantity;
            FlavorText = flavorText;
            EquipType = type;
        }

        public InventoryItem()
        {
            
        }
    }
}
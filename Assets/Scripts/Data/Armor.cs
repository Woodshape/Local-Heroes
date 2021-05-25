using UnityEngine;

namespace LH.Data {
    public enum ArmorType {
        Chest,
        Head,
        Legs,
        Arms
    }
    
    [CreateAssetMenu(fileName = "Armor", menuName = "LH/Items/Armor", order = 1)]
    public class Armor : Item, IEquippable {
        [Header("Armor")]
        public ArmorType type;

        public bool IsEquipped { get; set; }

        public void EquipItem() {
            IsEquipped = true;
            Debug.Log($"Armor of type {type} equipped!");
        }
    }
}

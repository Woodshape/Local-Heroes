using UnityEngine;

namespace LH.Data {
    public enum ArmorType {
        Chest,
        Head,
        Legs,
        Arms
    }
    
    [CreateAssetMenu(fileName = "Armor", menuName = "LH/Items/Armor", order = 1)]
    public class Armor : Item {
        [Header("Armor")]
        public ArmorType type;
    }
}

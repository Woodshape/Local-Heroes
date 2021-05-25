using LH.Stats;
using UnityEngine;

namespace LH.Data {
    public enum WeaponType {
        Melee
    }
    
    [CreateAssetMenu(fileName = "Weapon", menuName = "LH/Items/Weapon", order = 0)]
    public class Weapon : Item, IEquippable {
        [Header("Weapon")]
        public WeaponType type;

        public bool IsEquipped { get; set; }

        public void EquipItem() {
            IsEquipped = true;
            Debug.Log($"Weapon of type {type} equipped!");
        }
    }
}

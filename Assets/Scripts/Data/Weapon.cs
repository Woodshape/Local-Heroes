using LH.Stats;
using UnityEngine;

namespace LH.Data {
    public enum WeaponType {
        Melee
    }
    
    [CreateAssetMenu(fileName = "Weapon", menuName = "LH/Items/Weapon", order = 0)]
    public class Weapon : Item {
        [Header("Weapon")]
        public WeaponType type;
    }
}

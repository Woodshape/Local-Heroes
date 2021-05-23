using UnityEngine;

namespace DefaultNamespace {
    public interface IHumanoid {
        public GameObject WeaponPrefab { get; set; }
        public Transform WeaponHand { get; set; }
        public Weapon CurrentWeapon { get; set; }
    }
}

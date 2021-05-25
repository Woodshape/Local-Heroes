using System.Collections.Generic;
using LH.Data;
using LH.Stats;
using UnityEngine;

namespace DefaultNamespace {
    public class WeaponObject : MonoBehaviour {
        [SerializeField]
        private Weapon _weapon;

        public Weapon GetWeapon() {
            return _weapon;
        }
    }
}

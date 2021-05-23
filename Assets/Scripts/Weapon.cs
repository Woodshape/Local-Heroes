using System.Collections.Generic;
using LH.Stats;
using UnityEngine;

namespace DefaultNamespace {
    public class Weapon : MonoBehaviour, IStatModifier {
        [SerializeField]
        private float damage;
        
        public float GetDamage() {
            return damage;
        }
        
        public IEnumerable<float> GetModifier(Stat stat) {
            yield return damage;
        }
        
        public IEnumerable<float> GetMultiplier(Stat stat) {
            yield return 0f;
        }
    }
}

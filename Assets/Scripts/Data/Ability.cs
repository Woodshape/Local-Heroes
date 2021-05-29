using UnityEngine;

namespace LH.Data {
    public abstract class Ability : ScriptableObject {
        [SerializeField]
        private float _baseStrength;
        [SerializeField]
        private float _baseCost;

        public float GetStrength() {
            return _baseStrength;
        }
        
        public float GetCost() {
            return _baseCost;
        }
    }
}

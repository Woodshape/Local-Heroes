using UnityEngine;

namespace LH.Stats {
    public class StatModifier {
        public int ModifierIntValue { get; set; }

        public float ModifierFLoatValue { get; set; } = 1.0f;

        public StatModifier(int modifierValue) {
            Debug.Log("Modifying Integer: " + modifierValue);
            this.ModifierIntValue = modifierValue;
        }
        
        public StatModifier(float modifierValue) {
            Debug.Log("Modifying Float: " + modifierValue);
            this.ModifierFLoatValue = modifierValue;
        }
    }
}

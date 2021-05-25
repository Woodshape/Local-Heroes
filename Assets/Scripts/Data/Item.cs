using System;
using System.Collections.Generic;
using LH.Stats;
using UnityEngine;

namespace LH.Data {
    public class Item : ScriptableObject {
        [Serializable]
        public class ItemModifier {
            public Stat stat;
            public float value;
        }

        [SerializeField]
        protected List<ItemModifier> modifiers;
        [SerializeField]
        protected List<ItemModifier> multipliers;

        private Dictionary<Stat, float> modifierLookupTable;
        private Dictionary<Stat, float> multiplierLookupTable;
        
        public float GetModifier(Stat stat) {
            float value = 0f;

            if (this is IEquippable && ((IEquippable) this).IsEquipped) {
                BuildModifierLookupTable();
            
                foreach (KeyValuePair<Stat, float> mod in modifierLookupTable) {
                    if (mod.Key == stat) {
                        Debug.Log($"Getting item modifier for {stat} on item {this}: {mod.Value}");
                        value += mod.Value;
                    }
                }
            }

            return value;
        }
        
        public float GetMultiplier(Stat stat) {
            float value = 0f;

            if (this is IEquippable && ((IEquippable) this).IsEquipped) {
                BuildMultiplierLookupTable();

                foreach (KeyValuePair<Stat, float> mult in multiplierLookupTable) {
                    if (mult.Key == stat) {
                        value += mult.Value;
                    }
                }
            }
            
            return value;
        }

        private void BuildModifierLookupTable() {
            if (modifierLookupTable != null) {
                return;
            }

            modifierLookupTable = new Dictionary<Stat, float>();
            
            foreach (ItemModifier modifier in modifiers) {
                modifierLookupTable[modifier.stat] = modifier.value;
            }
        }

        private void BuildMultiplierLookupTable() {
            if (multiplierLookupTable != null) {
                return;
            }

            multiplierLookupTable = new Dictionary<Stat, float>();
            foreach (ItemModifier multiplier in multipliers) {
                multiplierLookupTable[multiplier.stat] = multiplier.value;
            }
        }
    }
}

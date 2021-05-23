using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LH.Stats {
    public class BaseStat {
        public string Name { get; set; }

        public string Description { get; set; }

        public float BaseValue { get; set; }

        private float _value;

        public List<StatModifier> StatModifiers { get; set; }

        public BaseStat(string name, string description, float baseValue) {
            Name = name;
            Description = description;
            BaseValue = baseValue;

            StatModifiers = new List<StatModifier>();

            Debug.Log($"Created stat {name} with value: {baseValue}");
        }

        public float GetValue() {
            _value += BaseValue;
            
            StatModifiers.ForEach(s => _value += s.ModifierIntValue);
            StatModifiers.ForEach(s => _value *= s.ModifierFLoatValue);

            return _value;
        }

        public void AddStatModifier(StatModifier modifier) {
            StatModifiers.Add(modifier);
        }
        
        public void RemoveStatModifier(StatModifier modifier) {
            StatModifiers.Remove(modifier);
        }
    }
}

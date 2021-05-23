using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LH.Stats {
    public class BaseStats : MonoBehaviour {
        [Range(1, 99)]
        [SerializeField]
        private int startingLevel = 1;
        [SerializeField] 
        private BaseClass baseClass;
        [SerializeField]
        private Progression progression;

        [SerializeField]
        private int[] experienceToLevel;
        private int currentLevel = 0;

        private Dictionary<Stat, IStatModifier> statModifiers;

        public delegate void LevelGainedDelegate();

        public event LevelGainedDelegate levelUpEvent;

        private void Start() {
            Experience experience = GetComponent<Experience>();
            if (experience) {
                experience.experienceGainEvent += onExperienceGained;
            }

            currentLevel = startingLevel;
            CalculateLevel();
        }

        public float GetStat(Stat stat) {
            float baseValue = GetBaseStatValue(stat);
            float modifiers = GetModifiers(stat);
            float multipliers = GetMultipliers(stat);

            Debug.Log("GET Stat: " + stat);
            Debug.Log("Base: " + baseValue);
            Debug.Log("Mod: " + modifiers);
            Debug.Log("Mult: " + multipliers);
            Debug.Log("VALUE: " + (baseValue + modifiers) * multipliers);
            
            //
            //  Add modifiers to base value FIRST before multiplication
            //  10 + 10 = 20
            //  20 * 1.1 = 22
            //
            return (baseValue + modifiers) * multipliers;
        }

        // public void AddStatModifier(Stat stat, IStatModifier modifier) {
        //     statModifiers.Add(stat, modifier);
        // }
        //
        // public void RemoveStatModifier(Stat stat, IStatModifier modifier) {
        //     Dictionary<Stat, IStatModifier> temp = new Dictionary<Stat, IStatModifier>();
        //     
        //     if (statModifiers.ContainsKey(stat)) {
        //         foreach (KeyValuePair<Stat, IStatModifier> stats in statModifiers) {
        //             if (!stats.Value.Equals(modifier)) {
        //                 temp.Add(stats.Key, stats.Value);
        //             }
        //         }
        //     }
        //
        //     statModifiers = temp;
        // }

        public int GetLevel() {
            if (currentLevel < 1) {
                currentLevel = CalculateLevel();
            }
            
            return currentLevel;
        }
        
        public void SetLevel(int level) {
            currentLevel = level;
        }

        public int GetExperienceNeeded() {
            return experienceToLevel[currentLevel - 1];
        }

        public void onExperienceGained() {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel) {
                Debug.Log(this.gameObject.name + " leveled up to level " + newLevel);
                
                currentLevel = newLevel;
                
                levelUpEvent?.Invoke();
            }
        }

        public int GetExperienceReward() {
            //  FIXME
            return Convert.ToInt32(startingLevel * 10 * Difficulties.GetDifficultyModifier(GetComponent<Entity>().difficulty));
        }
        
        private float GetBaseStatValue(Stat stat) {
            return progression.GetValueForStat(stat, baseClass, GetLevel());
        }
        
        private int CalculateLevel() {
            if (GetComponent<Experience>() == null) {
                return currentLevel > 0 ? currentLevel : startingLevel;
            }

            int experience = GetComponent<Experience>().GetExperience();
            int maxLevel = experienceToLevel.Length;

            for (int level = 1; level <= maxLevel; level++) {
                int experienceNeeded = experienceToLevel[level - 1];
                if (experienceNeeded > experience) {
                    return level;
                }
            }

            return maxLevel;
        }
        
        private float GetModifiers(Stat stat) {
            float mod = 0f;

            //
            //  Add all modifiers before applying them to the total
            //  8 + 2 = 10
            //  -8 + 2 = -6
            // 
            
            // foreach (KeyValuePair<Stat, IStatModifier> statModifier in statModifiers) {
            //     if (statModifier.Key == stat) {
            //         foreach (float modifier in statModifier.Value.GetModifier(stat)) {
            //             mod += modifier;
            //         }
            //     }
            // }
            
            
            foreach (IStatModifier modifierInterface in GetComponents<IStatModifier>()) {
                foreach (float modifier in modifierInterface.GetModifier(stat)) {
                    Debug.Log("mod -> " + modifier);
                    mod += modifier;
                }
            }
            
            foreach (IStatModifier modifierInterface in GetComponentsInChildren<IStatModifier>()) {
                foreach (float modifier in modifierInterface.GetModifier(stat)) {
                    Debug.Log("mod -> " + modifier);
                    mod += modifier;
                }
            }

            return mod;
        }
        
        private float GetMultipliers(Stat stat) {
            float mod = 0f;

            //
            //  Add all percentage modifiers before applying them to the total
            //  8% + 2% = 10%
            //  OR 0.08 + 0.02 = 0.01
            //  NOT: 0.08 * 0.02 = 0,0016
            // 
            int count = 0;
            foreach (IStatModifier multiplierInterface in GetComponents<IStatModifier>()) {
                foreach (float multiplier in multiplierInterface.GetMultiplier(stat)) {
                    count++;
                    
                    Debug.Log("mult -> " + multiplier);
                    mod += multiplier;
                }
            }
            foreach (IStatModifier multiplierInterface in GetComponentsInChildren<IStatModifier>()) {
                foreach (float multiplier in multiplierInterface.GetMultiplier(stat)) {
                    count++;
                    
                    Debug.Log("mult -> " + multiplier);
                    mod += multiplier;
                }
            }

            return count > 0 ? mod : 1f;
        }
    }
}


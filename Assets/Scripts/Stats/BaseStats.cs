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
        private int currentLevel;

        private void Start() {
            Experience experience = GetComponent<Experience>();
            if (experience) {
                experience.experienceGainEvent += onExperienceGained;
            }

            currentLevel = startingLevel;
            CalculateLevel();
        }

        public int GetHealth() {
            return progression.GetValueForStat(Stats.Health, baseClass, currentLevel);
        }
        
        public int GetDamage() {
            return progression.GetValueForStat(Stats.Damage, baseClass, currentLevel);
        }

        public int GetLevel() {
            if (currentLevel < 1) {
                currentLevel = CalculateLevel();
            }
            
            return currentLevel;
        }

        public int CalculateLevel() {
            if (GetComponent<Experience>() == null) {
                return startingLevel;
            }

            int experience = GetComponent<Experience>().GetExperience();
            int maxLevel = experienceToLevel.Length;

            for (int level = 0; level <= maxLevel; level++) {
                int experienceNeeded = experienceToLevel[level];
                if (experienceNeeded > experience) {
                    return level;
                }
            }

            return maxLevel + 1;
        }

        public void onExperienceGained() {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel) {
                Debug.Log(this.gameObject.name + " leveled up to level " + newLevel);

                currentLevel = newLevel;
            }
        }

        public int GetExperienceReward() {
            //  FIXME
            return Convert.ToInt32(startingLevel * 10 * Difficulties.GetDifficultyModifier(GetComponent<Entity>().difficulty));
        }
    }
}


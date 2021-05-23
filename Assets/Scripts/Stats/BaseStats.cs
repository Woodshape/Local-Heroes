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

        public int GetStat(Stats stat) {
            return progression.GetValueForStat(stat, baseClass, GetLevel());
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

            for (int level = 1; level <= maxLevel; level++) {
                int experienceNeeded = experienceToLevel[level - 1];
                if (experienceNeeded > experience) {
                    return level;
                }
            }

            return maxLevel;
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
    }
}


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LH.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "LH/Stats/Progression", order = 0)]
    public class Progression : ScriptableObject {
        [Serializable]
        class ProgressionClass {
            public BaseClass baseClass;
            public ProgressionStat[] stats;
        }

        [Serializable]
        class ProgressionStat {
            public Stat stat;
            public float[] valueForLevels;
        }
        

        [SerializeField]
        private ProgressionClass[] progressionClasses;

        private Dictionary<BaseClass, Dictionary<Stat, float[]>> lookupTable;

        public float GetValueForStat(Stat stat, BaseClass baseClass, int level) {
            BuildLookupTable();
            
            Debug.Log($"Getting {stat} for level {level} of class {baseClass}:");

            float[] levels = lookupTable[baseClass][stat];
            if (levels.Length < level) {
                Debug.LogWarning($"No entry found: {stat} for level {level} of class {baseClass}");
                return 0;
            }

            return levels[level - 1];
        }
        
        private void BuildLookupTable() {
            if (lookupTable != null) {
                return;
            }

            Debug.Log("Building new lookup table");

            lookupTable = new Dictionary<BaseClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionClass progressionClass in progressionClasses) {
                Dictionary<Stat, float[]> innerLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats) {
                    innerLookupTable[progressionStat.stat] = progressionStat.valueForLevels;
                }
                
                lookupTable[progressionClass.baseClass] = innerLookupTable;
            }
        }
    }
}
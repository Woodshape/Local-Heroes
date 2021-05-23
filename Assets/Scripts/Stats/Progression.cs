using System;
using System.Collections.Generic;
using UnityEngine;

namespace LH.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "LH/Stats/Progression", order = 0)]
    public class Progression : ScriptableObject {
        [Serializable]
        class ProgressionClass {
            public BaseClass baseClass;
            public ProgressionStat[] stats;
        }
        
        [Serializable]
        class ProgressionLevel {
            public int level;
            public ProgressionStat stat;
        }

        [Serializable]
        class ProgressionStat {
            public Stats stat;
            public int[] valueForLevels;
        }
        

        [SerializeField]
        private ProgressionClass[] progressionClasses;

        private Dictionary<BaseClass, Dictionary<Stats, int[]>> lookupTable;

        public int GetValueForStat(Stats stat, BaseClass baseClass, int level) {
            BuildLookupTable();
            
            Debug.Log($"Getting {stat} for level {level} of class {baseClass}:");

            int[] levels = lookupTable[baseClass][stat];
            if (levels.Length < level) {
                Debug.LogWarning($"No entry found: {stat} for level {level} of class {baseClass}");
                return 0;
            }

            return levels[level -1];
        }
        private void BuildLookupTable() {
            if (lookupTable != null) {
                return;
            }

            lookupTable = new Dictionary<BaseClass, Dictionary<Stats, int[]>>();

            foreach (ProgressionClass progressionClass in progressionClasses) {
                Dictionary<Stats, int[]> innerLookupTable = new Dictionary<Stats, int[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats) {
                    innerLookupTable[progressionStat.stat] = progressionStat.valueForLevels;
                }
                
                lookupTable[progressionClass.baseClass] = innerLookupTable;
            }
        }
    }
}
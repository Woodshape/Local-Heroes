using System;
using System.Collections.Generic;
using UnityEngine;

namespace LH.Stats {
    public class EntityStats : MonoBehaviour {
        public EntityTemplate template;
        public Dictionary<Stats, BaseStat> stats = new Dictionary<Stats, BaseStat>();

        private void Start() {
            BaseClass baseClass = template.baseClass;

            foreach (StatDict stat in template.stats) {
                stats.Add(stat.stat, new BaseStat(stat.stat.ToString(), "", stat.startingValue));
            }
        }

        public float GetValue(Stats stat) {
            if (stats.ContainsKey(stat)) {
                stats[stat].GetValue();
            }

            return -1;
        }
    }
}

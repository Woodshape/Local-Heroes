using System;
using System.Collections.Generic;
using UnityEngine;

namespace LH.Stats {
    public class EntityStats : MonoBehaviour {
        public EntityTemplate template;
        public Dictionary<Stat, BaseStat> stats = new Dictionary<Stat, BaseStat>();

        private void Start() {
            BaseClass baseClass = template.baseClass;

            foreach (StatDict stat in template.stats) {
                stats.Add(stat.stat, new BaseStat(stat.stat.ToString(), "", stat.startingValue));
            }
        }

        public float GetValue(Stat stat) {
            if (stats.ContainsKey(stat)) {
                stats[stat].GetValue();
            }

            return -1;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LH.Stats {

    [Serializable]
    public class StatDict {
        [FormerlySerializedAs("stat")]
        public Stat stat;
        public float startingValue;
        public float levelModifier;
    }
    
    [CreateAssetMenu(fileName = "EntityTemplate", menuName = "LH/Stats/Template", order = 3)]
    public class EntityTemplate : ScriptableObject {
        public BaseClass baseClass;
        public List<StatDict> stats = new List<StatDict>();
    }
}

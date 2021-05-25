using System;
using System.Collections.Generic;
using LH.GOAP;
using LH.Stats;
using UnityEngine;

namespace DefaultNamespace {
    public class GameWorld : MonoBehaviour {
        public static GameWorld Instance;
        
        public GWorld GWorld;

        private void Start() {
            if (Instance == null) {
                Instance = this;
                
                DontDestroyOnLoad(this);
            }
            
            GWorld = GWorld.Instance;
        }
        
        public int CalculateAverageLevel(Resource resource) {
            Queue<GameObject> entity = GWorld.GetQueue(resource.ToString()).queue;

            int levels = 0;
            foreach (GameObject character in entity) {
                levels += character.GetComponent<BaseStats>().GetLevel();
            }

            return levels / (entity.Count != 0 ? entity.Count : 1);
        }
    }
}

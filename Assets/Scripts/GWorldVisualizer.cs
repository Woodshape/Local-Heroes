using System.Collections.Generic;
using LH.GOAP;
using UnityEngine;

namespace DefaultNamespace {
    
    [ExecuteInEditMode]
    public class GWorldVisualizer : MonoBehaviour {
        public GWorld world;

        [SerializeField]
        public Dictionary<string, int> worldObj;
        
        void Start()
        {
            world = GWorld.Instance;
            worldObj = world.GetWorldStates().GetStates();
        }
    }
}

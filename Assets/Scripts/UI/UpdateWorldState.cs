using System.Collections.Generic;
using LH.GOAP;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class UpdateWorldState : MonoBehaviour {
        // Storage for the states
        public Text states;

        void LateUpdate() {
            // Dictionary of states
            Dictionary<string, int> worldStatesDict = GWorld.Instance.GetWorldStates().GetStates();
            // Clear out the states text
            states.text = "";
            // Cycle through them all and store in states.text
            foreach (KeyValuePair<string, int> s in worldStatesDict) {

                states.text += s.Key + ", " + s.Value + "\n";
            }
        }
    }
}

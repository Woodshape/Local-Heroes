using System;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace LH.GOAP
{
    public abstract class GAction : MonoBehaviour
    {
        // Name of the action
        public string actionName = "Action";
        // Cost of the action
        public float cost = 1.0f;
        // Target where the action is going to take place
        public GameObject destinationGO;
        // Store the tag
        public string destinationTag;
        // Duration the action should take
        public float duration = 0.0f;
        // An array of WorldStates of preconditions
        public GState[] preConditions;
        // An array of WorldStates of afterEffects
        public GState[] afterEffects;
        // The NavMEshAgent attached to the agent
        public NavMeshAgent navMeshAgent;
        // Dictionary of preconditions
        public Dictionary<string, int> preconditionsDict;
        // Dictionary of effects
        public Dictionary<string, int> effectsDict;
        
        // State of the agent
        public GStates agentBeliefs;
        // Access our inventory
        public GInventory agentInventory;
        
        // Are we currently performing an action?
        public bool IsRunning = false;

        // Constructor
        protected GAction()
        {
            // Set up the preconditions and effects
            preconditionsDict = new Dictionary<string, int>();
            effectsDict = new Dictionary<string, int>();
        }

        private void Awake() {
            // Get hold of the agents NavMeshAgent
            navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();

            // Check if there are any preConditions in the Inspector
            // and add to the dictionary
            if (preConditions != null) {
                foreach (GState w in preConditions) {

                    // Add each item to our Dictionary
                    preconditionsDict.Add(w.key, w.value);
                }
            }

            // Check if there are any afterEffects in the Inspector
            // and add to the dictionary
            if (afterEffects != null) {

                foreach (GState w in afterEffects) {

                    // Add each item to our Dictionary
                    effectsDict.Add(w.key, w.value);
                }
            }
            // Populate our inventory
            agentInventory = this.GetComponent<GAgent>().Inventory;
            // Get our agents beliefs
            agentBeliefs = this.GetComponent<GAgent>().Beliefs;
        }
        
        public virtual bool IsAchievable() {
            return true;
        }
        
        //check if the action is achievable given the condition of the
        //world and trying to match with the actions preconditions
        public bool IsAchievableGiven(Dictionary<string, int> conditions) {
            foreach (KeyValuePair<string, int> p in preconditionsDict) {
                if (!conditions.ContainsKey(p.Key)) {
                    return false;
                }
            }
            return true;
        }

        public abstract void Perform();
        public abstract bool PrePerform();
        public abstract bool PostPerform();
    }
}

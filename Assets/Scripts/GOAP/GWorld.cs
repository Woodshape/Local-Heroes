using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using UnityEngine;

namespace LH.GOAP {
    public class ResourceQueue {

        public Queue<GameObject> queue = new Queue<GameObject>();
        public string tag;
        public string modState;

        public ResourceQueue(string t, string ms, GStates w) {
            tag = t;
            modState = ms;
            if (tag != "") {
                GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject r in resources) {
                    queue.Enqueue(r);
                }
            }

            if (modState != "") {
                w.ModifyState(modState, queue.Count);
            }
        }

        // Add the resource
        public void AddResource(GameObject r, bool identity) {
            if (identity && queue.Contains(r)) {
                return;
            }
            
            queue.Enqueue(r);
        }


        // Remove the resource
        public GameObject GetNextResource() {
            if (queue.Count == 0) return null;

            return queue.Dequeue();
        }

        // Overloaded RemoveResource
        public void RemoveResource(GameObject r) {
            // Put everything in a new queue except 'r' and copy it back to que
            queue = new Queue<GameObject>(queue.Where(p => p != r));
        }
    }
    
    public class GWorld {
        // Our GWorld instance
        public static GWorld Instance { get; } = new GWorld();
        
        // Our world states
        private static GStates worldStates;
        // Queue of characters
        private static ResourceQueue characters;
        // Queue of characters
        private static ResourceQueue creatures;
        
        // Storage for all
        private static Dictionary<string, ResourceQueue> resourcesDict = new Dictionary<string, ResourceQueue>();
        
        static GWorld() {
            // Create our world
            worldStates = new GStates();
            
            // Create character array and add to the resources Dictionary
            characters = CreateAndAddResource("", "characters", Resource.CHARACTER);

            // Create creature array and add to the resources Dictionary
            creatures = CreateAndAddResource("", "creatures", Resource.CREATURE);

            Debug.Log("Added resources to world: ");
            foreach (KeyValuePair<string, ResourceQueue> resource in resourcesDict) {
                Debug.Log(resource.Key + " " + resource.Value.tag + " | " + resource.Value.modState);
            }

            // Set the time scale in Unity
            Time.timeScale = 2.0f;
        }
        private static ResourceQueue CreateAndAddResource(string type, string state, Resource resource) {
            ResourceQueue queue = new ResourceQueue(type, state, worldStates);
            resourcesDict.Add(resource.ToString(), queue);

            return queue;
        }

        public ResourceQueue GetQueue(string type) {
            return resourcesDict[type];
        }
        
        public GStates GetWorldStates() {
            return worldStates;
        }

        public Dictionary<string, ResourceQueue> GetResources() {
            return resourcesDict;
        }
    }
}

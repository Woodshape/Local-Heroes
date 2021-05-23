using System;
using System.Collections.Generic;
using UnityEngine;

namespace LH.GOAP {
    //make the dictionary elements their own serializable class
    //so we can edit them in the inspector
    [Serializable]
    public class GState {
        public string key;
        public int value;
    }

    public class GStates {
        // Constructor
        private readonly Dictionary<string, int> _statesDict;

        public GStates() {
            _statesDict = new Dictionary<string, int>();
        }

        /************** Helper functions ****************/
        // Check for a key
        public bool HasState(string key) {
            return _statesDict.ContainsKey(key);
        }

        // Add to our dictionary
        private void AddState(string key, int value) {
            if (value < 0) {
                return;
            }
            
            Debug.Log($"Adding state {key} to states with value: {value}");
            _statesDict.Add(key, value);
        }

        public void ModifyState(string key, int value) {
            // If it contains this key
            if (HasState(key)) {
                int oldValue = _statesDict[key];
                // Add the value to the state
                _statesDict[key] += value;

                // Debug.Log($"Modifying state {key} by value {value}: {oldValue} -> {_statesDict[key]}");
                
                // If it's less than zero then remove it
                if (_statesDict[key] <= 0) { // Call the RemoveState method
                    RemoveState(key);
                }
            } else {
                AddState(key, value);
            }
        }

        // Method to remove a state
        public void RemoveState(string key) {
            // Check if it exists
            if (HasState(key)) {
                Debug.Log($"Removing state {key} from states");
                _statesDict.Remove(key);
            }
        }

        // Set a state
        public void SetState(string key, int value) {
            // Check if it exists
            if (HasState(key)) {
                _statesDict[key] = value;
            } else {
                AddState(key, value);
            }
        }

        public Dictionary<string, int> GetStates() {
            return _statesDict;
        }
    }
}

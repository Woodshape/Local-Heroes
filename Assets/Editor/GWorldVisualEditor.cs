using System.Collections.Generic;
using DefaultNamespace;
using LH.GOAP;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LH.Editor {
    [CustomEditor(typeof(GWorldVisualizer))]
    [CanEditMultipleObjects]
    public class GWorldVisualEditor : UnityEditor.Editor {
        void OnEnable() {
        }
        
        //Display properties of the GWorld in the Inspector
        public override void OnInspectorGUI() {
            //Draw the default items in the Inspector as Unity would without
            //this script
            DrawDefaultInspector();

            //syncronise values from the running of the code eith the script properties
            serializedObject.Update();

            GWorldVisualizer world = (GWorldVisualizer)target;
            
            GUILayout.Label("States: ");
            foreach (KeyValuePair<string, int> state in world.world.GetWorldStates().GetStates()) {
                GUILayout.Label("---: ");
                GUILayout.Label("=====  " + state.Key + " " + state.Value);
            }
            
            GUILayout.Label("Resources: ");
            foreach (KeyValuePair<string, ResourceQueue> resource in world.world.GetResources()) {
                GUILayout.Label("---: ");
                GUILayout.Label("C: " + resource.Value.queue.Count);
                foreach (GameObject res in resource.Value.queue) {
                    GUILayout.Label("=====  " + resource.Key + " " + res.name);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

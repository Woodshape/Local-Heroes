using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using LH.GOAP;
using UnityEditor;
using UnityEngine;

namespace LH.Editor {
    [CustomEditor(typeof(GAgentVisualizer))]
    [CanEditMultipleObjects]
    public class GAgentVisualEditor : UnityEditor.Editor {


        void OnEnable() { }

        //Display properties of the GAgent in the Inspector
        public override void OnInspectorGUI() {
            //Draw the default items in the Inspector as Unity would without
            //this script
            DrawDefaultInspector();

            //syncronise values from the running of the code eith the script properties
            serializedObject.Update();

            //get the agent game object so the GAgent and associated properties can
            //be displayed
            GAgentVisualizer agent = (GAgentVisualizer) target;
            GUILayout.Label("Name: " + agent.name);
            GUILayout.Label("Current Action: " + agent.gameObject.GetComponent<GAgent>().CurrentAction);
            GUILayout.Label("Actions: ");
            foreach (GAction a in agent.gameObject.GetComponent<GAgent>().Actions) {
                string pre = "";
                string eff = "";

                foreach (KeyValuePair<string, int> p in a.preconditionsDict)
                    pre += p.Key + ", ";

                foreach (KeyValuePair<string, int> e in a.effectsDict)
                    eff += e.Key + ", ";

                GUILayout.Label("====  " + a.actionName + "(" + pre + ")(" + eff + ")");
            }

            GUILayout.Label("Goals: ");
            foreach (KeyValuePair<Goal, int> g in agent.gameObject.GetComponent<GAgent>().GoalsDict) {
                GUILayout.Label("---: ");
                foreach (KeyValuePair<string, int> sg in g.Key.goalsDict)
                    GUILayout.Label("=====  " + sg.Key);
            }

            GUILayout.Label("Beliefs: ");
            foreach (KeyValuePair<string, int> sg in agent.gameObject.GetComponent<GAgent>().Beliefs.GetStates()) {
                GUILayout.Label("=====  " + sg.Key);
            }

            GUILayout.Label("Inventory: ");
            foreach (GameObject g in agent.gameObject.GetComponent<GAgent>().Inventory.items) {
                if (g != null) {
                    GUILayout.Label("====  " + g.tag);
                }
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}

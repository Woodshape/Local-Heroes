using UnityEditor;
using UnityEngine;

namespace LH.Editor {
    public class ExtendedEditorWindow : EditorWindow {
        protected SerializedObject serializedObject;
        protected SerializedProperty serializedProperty;

        protected void DrawProperties(SerializedProperty property, bool drawChildren) {
            string lastPropPath = string.Empty;
            foreach (SerializedProperty prop in property) {
                Debug.Log(prop.name);
                if (prop.isArray && prop.propertyType == SerializedPropertyType.Generic) {
                    
                    EditorGUILayout.BeginHorizontal();
                    prop.isExpanded = EditorGUILayout.Foldout(prop.isExpanded, prop.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (prop.isExpanded) {
                        EditorGUI.indentLevel++;
                        DrawProperties(prop, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else {
                    if (!string.IsNullOrEmpty(lastPropPath) && prop.propertyPath.Contains(lastPropPath)) {
                        continue;
                    }

                    lastPropPath = prop.propertyPath;
                    EditorGUILayout.PropertyField(prop, drawChildren);
                }
            }
        }
    }
}

using System;
using LH.Editor;
using LH.Data;
using UnityEditor;
using UnityEngine;

namespace LH.Editor {
    public class GameDataObjectEditorWindow : ExtendedEditorWindow {
        public static void Open(GameDataObject dataObject) {
            GameDataObjectEditorWindow window = GetWindow<GameDataObjectEditorWindow>("Game Data Editor");
            window.serializedObject = new SerializedObject(dataObject);
        }

        private void OnGUI() {
            serializedProperty = serializedObject.FindProperty("gameData");
            DrawProperties(serializedProperty, true);
        }
    }
}

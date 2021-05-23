using LH.Data;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace LH.Editor {
    public class AssetHandler {
        [OnOpenAsset]
        public static bool OpenEditor(int instanceId, int line) {
            GameDataObject obj = EditorUtility.InstanceIDToObject(instanceId) as GameDataObject;
            if (obj != null) {
                GameDataObjectEditorWindow.Open(obj);

                return true;
            }

            return false;
        }
    }

//    [CustomEditor(typeof(GameDataObject))]
    public class GameDataObjectCustomEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            if (GUILayout.Button("Open Editor")) {
                GameDataObjectEditorWindow.Open((GameDataObject) target);
            }
        }
    }
}

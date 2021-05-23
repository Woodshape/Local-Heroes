using System;
using System.Collections.Generic;
using UnityEngine;

namespace LH.Data {
    [Serializable]
    public class GameData {
        public string playername;
        public GameObject obj;
    }
    
    [CreateAssetMenu(fileName = "Game Data", menuName = "LH/Data/Game Data", order = 0)]
    public class GameDataObject : ScriptableObject {
        [SerializeField]
        public List<GameData> gameData;
    }
}

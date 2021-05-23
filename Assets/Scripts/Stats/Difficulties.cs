using System.Collections.Generic;

namespace LH.Stats {
    public enum Difficulty {
            Trivial,
            Easy,
            Normal,
            Challenging,
            Hard,
            Extreme
    }
    
    public static class Difficulties {
        private static readonly Dictionary<Difficulty, float> modifierDict = new Dictionary<Difficulty, float>();

        static Difficulties() {
            modifierDict.Add(Difficulty.Trivial, 0.5f);
            modifierDict.Add(Difficulty.Easy, 0.75f);
            modifierDict.Add(Difficulty.Normal, 1.0f);
            modifierDict.Add(Difficulty.Challenging, 1.25f);
            modifierDict.Add(Difficulty.Hard, 1.5f);
            modifierDict.Add(Difficulty.Extreme, 2.0f);
        }

        public static float GetDifficultyModifier(Difficulty difficulty) {
            return modifierDict[difficulty];
        }
    }
}

using UnityEngine;

namespace LH.Stats {
    public class Experience : MonoBehaviour {
        [SerializeField]
        private int experience;

        public delegate void ExperienceGainedDelegate();

        public event ExperienceGainedDelegate experienceGainEvent;

        public void GainExperience(int exp) {
            int amount = Mathf.Abs(exp);

            experience += amount;
            
            experienceGainEvent?.Invoke();
        }

        public int GetExperience() => experience;
    }
}

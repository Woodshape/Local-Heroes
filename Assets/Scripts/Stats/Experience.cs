using System;
using UnityEngine;
using UnityEngine.UI;

namespace LH.Stats {
    public class Experience : MonoBehaviour {
        
        public Slider experienceSlider;
        
        private int experience;

        public delegate void ExperienceGainedDelegate();

        public event ExperienceGainedDelegate experienceGainEvent;

        private void Update() {
            if (experienceSlider == null) {
                return;
            }

            if (experience != 0) {
                    experienceSlider.value = (float) experience / GetComponent<BaseStats>().GetExperienceNeeded(); 
            } 
            else {
                experienceSlider.value = 0;
            }
        }

        public void GainExperience(int exp) {
            int amount = Mathf.Abs(exp);

            experience += amount;
            
            experienceGainEvent?.Invoke();
        }

        public int GetExperience() => experience;
    }
}

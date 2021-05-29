using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LH.GOAP;
using LH.Stats;
using TMPro;
using UnityEngine;

namespace LH.UI {
    public class EntityInformationDisplay : MonoBehaviour {

        [SerializeField]
        TextMeshProUGUI nameText;
        [SerializeField]
        TextMeshProUGUI levelText;
        [SerializeField]
        TextMeshProUGUI experienceText;
        [SerializeField]
        TextMeshProUGUI healthText;
        [SerializeField] 
        TextMeshProUGUI actionText;

        public bool shouldUpdateActionInformation;
        
        private Entity _entity;
        private Health _health;
        private Experience _experience;

        public void DisplayInformation(Entity entity) {
            _entity = entity;

            _health = _entity.GetComponent<Health>();
            if (_health) {
                _health.healthChangedEvent += UpdateInformation;
                _health.deathEvent += health => ResetInformation();
            }

            _experience = _entity.GetComponent<Experience>();
            if (_experience) {
                _experience.experienceGainEvent += UpdateInformation;
            }
            
            _entity.actionChangeEvent += UpdateActionInformation;
            
            UpdateInformation();
        }
        
        private void UpdateInformation() {
            if (_entity == null) {
                return;
            }
            
            nameText.text = _entity.entityName;

            if (_entity.GetComponent<BaseStats>()) {
                levelText.text = _entity.GetComponent<BaseStats>().GetLevel().ToString();
            }

            if (_experience) {
                experienceText.text = _experience.GetExperience().ToString();
            }
            else {
                experienceText.text = "---";
            }

            if (_health) {
                healthText.text = $"{_health.GetCurrentHealth()} / {_entity.GetComponent<BaseStats>().GetStat(Stat.Health)}";
            }

            UpdateActionInformation();
        }

        private void UpdateActionInformation() {
            if (shouldUpdateActionInformation) {
                GAction action = _entity.CurrentAction;
                if (action == null) {
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(action.actionName);
                sb.AppendLine(action.IsRunning ? "IN PROGRESS" : "IDLE");
                
                actionText.text = sb.ToString();
            }
        }

        public void ResetInformation() {
            nameText.text = "---";
            levelText.text = "---";
            experienceText.text = "---";

            actionText.text = "---";
        }
    }
}

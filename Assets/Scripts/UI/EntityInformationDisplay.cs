using System;
using System.Collections;
using System.Collections.Generic;
using LH.Stats;
using TMPro;
using UnityEngine;

namespace LH.UI {
    public class EntityInformationDisplay : MonoBehaviour {

        [SerializeField]
        TextMeshProUGUI nameText;
        [SerializeField]
        private TextMeshProUGUI levelText;
        [SerializeField]
        TextMeshProUGUI experienceText;
        
        private Entity _entity;

        public void DisplayInformation(Entity entity) {
            _entity = entity;
        }

        private void Update() {
            if (_entity == null) {
                return;
            }
            
            nameText.text = _entity.entityName;

            if (_entity.GetComponent<BaseStats>()) {
                levelText.text = _entity.GetComponent<BaseStats>().GetLevel().ToString();
            }

            if (_entity.GetComponent<Experience>()) {
                experienceText.text = _entity.GetComponent<Experience>().GetExperience().ToString();
            }
            else {
                experienceText.text = "---";
            }
        }

        public void ResetInformation() {
            nameText.text = "---";
            experienceText.text = "---";
        }
    }
}

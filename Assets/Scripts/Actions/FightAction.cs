using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using LH.Data;
using LH.GOAP;
using UnityEngine;

namespace LH.Actions {
    public class FightAction : GAction {
        public string attackTrigger;
        public Animation attackAnimation;

        public Ability ability;
        
        private Entity _entity;

        private void Start() {
            _entity = GetComponent<Entity>();
        }

        private void Update() {
            if (!IsRunning && destinationGO != null || _entity != null) {
                IsRunning = true;
            }
        }

        public override void Perform() {
            if (destinationGO == null || GetComponent<Entity>().target == null) {
                GetComponent<Entity>().RemoveSelfFromCombat();
            }

            if (_entity) {
                _entity.AttackBehaviour(ability, attackTrigger, attackAnimation);
            }
        }

        public override bool PrePerform() {
            if (destinationGO == null || _entity == null) {
                return false;
            }
            
            Focus focus = _entity.GetComponent<Focus>();
            if (focus&& ability) {
                focus.SpendFocus(ability.GetCost());
            }

            Entity targetEntity = destinationGO.GetComponent<Entity>();

            targetEntity.Beliefs.ModifyState("isAttacked", 1);

            duration = _entity.attackSpeed;
            Debug.Log("Attack Duration: " + duration);

            agentInventory.AddItem(destinationGO);

            this.transform.LookAt(destinationGO.transform);

            return true;
        }

        public override bool PostPerform() {
            if (destinationGO != null) {
                agentInventory.RemoveItem(destinationGO);

                destinationGO.GetComponent<Entity>().Beliefs.RemoveState("isAttacked");
            }

            return true;
        }

        public override bool IsAchievable() {
            Focus focus = gameObject.GetComponent<Focus>();
            if (focus && ability) {
                return focus.GetCurrentFocus() >= ability.GetCost();
            }

            return true;
        }

        public void ClearTarget() {
            Debug.Log("Target dies... Removing it...");
            if (destinationGO != null) {
                destinationGO.GetComponent<Entity>().Beliefs.RemoveState("isAttacked");

                agentInventory.RemoveItem(destinationGO);
                destinationGO = null;
            }

            agentBeliefs.RemoveState("isFighting");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using LH.GOAP;
using UnityEngine;

namespace LH.Actions {
    public class FightAction : GAction {
        public Animation attackAnimation;
        private Entity entity;

        private void Start() {
            entity = GetComponent<Entity>();
        }

        private void Update() {
            if (!IsRunning && destinationGO != null || entity != null) {
                IsRunning = true;
            }
        }

        public override void Perform() {
            if (destinationGO == null || GetComponent<Entity>().target == null) {
                GetComponent<Entity>().RemoveSelfFromCombat();
            }

            if (entity) {
                entity.AttackBehaviour(attackAnimation);
            }
        }

        public override bool PrePerform() {
            if (destinationGO == null || entity == null) {
                return false;
            }

            Entity targetEntity = destinationGO.GetComponent<Entity>();

            //  subscribe to target death event
            // targetEntity.entityDeathEvent += onEntityDeath;

            targetEntity.Beliefs.ModifyState("isAttacked", 1);

            duration = entity.attackSpeed;
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

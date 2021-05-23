using System;
using System.Collections;
using DefaultNamespace;
using LH.GOAP;
using UnityEngine;

namespace LH.Actions {
    public class LookForTarget : GAction {
        [Header("Target")]
        public float range = 10f;
        public Resource resource;
        
        public override void Perform() {
            if (destinationGO != null) {
                float dist = Vector3.Distance(destinationGO.transform.position, this.transform.position);
                if (dist < range) {
                    StartFight();
                }
            }
            else {
                IsRunning = false;
            }
        }
        
        public override bool PrePerform() {
            GameObject closest = null;
            float closestDist = float.MaxValue;
            foreach (GameObject character in GWorld.Instance.GetQueue(resource.ToString()).queue) {
                float dist = Vector3.Distance(character.transform.position, this.transform.position);
                if (dist < range && dist < closestDist) {
                    if (CheckTarget(character)) {
                        closestDist = dist;
                        closest = character;
                    }
                }
            }

            if (closest) {
                destinationGO = closest;
                    
                StartFight();

                return true;
            }
            

            Debug.Log("No target within aggro range");
            agentBeliefs.ModifyState("noTargetInSight", 1);
            
            return false;
        }

        public override bool PostPerform() {
            return true;
        }

        private void StartFight() {
            destinationGO.GetComponent<Entity>().FightWith(this.gameObject);
            GetComponent<Entity>().FightWith(destinationGO.gameObject);
            
            agentBeliefs.RemoveState("noTargetInSight");
        }
        
        private bool CheckTarget(GameObject target) {
            Entity entity = target.GetComponent<Entity>();
            
            //  FIXME
            if (entity.target == null) {
                return true;
            }
            
            return false;
        }
    }
}

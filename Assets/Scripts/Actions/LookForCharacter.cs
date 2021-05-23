using System;
using System.Collections;
using DefaultNamespace;
using LH.GOAP;
using UnityEngine;

namespace LH.Actions {
    public class LookForCharacter : GAction {
        public float range = 10f;
        
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
            foreach (GameObject character in GWorld.Instance.GetQueue(Resource.CHARACTER.ToString()).queue) {
                float dist = Vector3.Distance(character.transform.position, this.transform.position);
                if (dist < range) {
                    if (dist < closestDist) {
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
            

            Debug.Log("No character within aggro range");
            agentBeliefs.ModifyState("noCharacterInSight", 1);
            
            return false;
        }
        
        public override bool PostPerform() {
            return true;
        }

        private void StartFight() {
            destinationGO.GetComponent<Character>().FightWith(this.gameObject);
            GetComponent<Creature>().FightWith(destinationGO.gameObject);
            
            agentBeliefs.RemoveState("noCharacterInSight");
        }
    }
}

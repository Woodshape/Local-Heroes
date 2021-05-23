using DefaultNamespace;
using LH.GOAP;
using UnityEngine;

namespace LH.Actions {
    public class LookForCreature : GAction {
        public override void Perform() {
            if (destinationGO == null) {
                IsRunning = false;
            }
        }
        
        public override bool PrePerform() {
            destinationGO = GWorld.Instance.GetQueue(Resource.CREATURE.ToString()).GetNextResource();
            if (destinationGO == null) {
                Debug.Log("No target found for of type: " + Resource.CREATURE);
                return false;
            }

            Debug.Log("Target found " + destinationGO.name);
            StartFight();

            return true;
        }
        
        public override bool PostPerform() {
            return true;
        }

        private void StartFight() {
            if (destinationGO) {
                destinationGO.GetComponent<Creature>().FightWith(this.gameObject);
                GetComponent<Character>().FightWith(destinationGO.gameObject);
            }
        }
    }
}

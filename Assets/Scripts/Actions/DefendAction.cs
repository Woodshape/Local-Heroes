using System.Collections;
using System.Collections.Generic;
using LH.GOAP;
using UnityEngine;

namespace LH.Actions {
    public class DefendAction : GAction {

        public override void Perform() {
            //StartCoroutine(Evade());
            Debug.Log("Evading...");
        }

        public override bool PrePerform() {
            this.destinationGO = GetComponent<Entity>().target.gameObject;

            if (destinationGO == null || !GetComponent<Entity>().HasEnoughStamina(50)) {
                return false;
            }

            GetComponent<Entity>().RemoveStamina(50);

            return true;
        }

        public override bool PostPerform() {
            return true;
        }

        IEnumerator Evade() {
            bool active = gameObject.activeSelf;
            gameObject.SetActive(!active);

            yield return new WaitForSeconds(.5f);
        }
    }
}

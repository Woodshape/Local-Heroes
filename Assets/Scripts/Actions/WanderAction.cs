using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using LH.GOAP;
using UnityEngine;
using UnityEngine.AI;

namespace LH.Actions {
    public class WanderAction : GAction {
        public override void Perform() { }

        public override bool PrePerform() {
            GameObject temp = new GameObject("Wanderpoint " + GetComponent<Entity>().entityName);
            temp.transform.position = new Vector3(transform.position.x + Random.Range(-10f, 10f), transform.position.y,
                transform.position.z + Random.Range(-10f, 10f));

            NavMeshPath path = new NavMeshPath();
            if (navMeshAgent.CalculatePath(temp.transform.position, path)) {
                destinationGO = temp;
            }
            else {
                Destroy(temp);
                return false;
            }

            duration = Random.Range(1f, 30f);

            return true;
        }

        public override bool PostPerform() {
            Destroy(destinationGO);

            agentBeliefs.RemoveState("noCharacterInSight");

            return true;
        }
    }
}

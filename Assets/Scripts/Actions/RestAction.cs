using System;
using System.Collections;
using System.Collections.Generic;
using LH.GOAP;
using LH.Stats;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace LH.Actions {
    public class RestAction : GAction {

        public GameObject campfireGO;

        private GameObject campfirePlace;

        private bool campfireSpawned;
        private float timeSinceLastHeal;

        private void Update() {
            timeSinceLastHeal += Time.deltaTime;

            if (!IsRunning || destinationGO == null) {
                return;
            }

            if (agentBeliefs.HasState("isFighting") || agentBeliefs.HasState("isAttacked")) {
                GetComponent<GAgent>().StopMoving();
                IsRunning = false;
            }
        }
        public override void Perform() {
            if (timeSinceLastHeal > 1f) {
                timeSinceLastHeal = 0f;

                Health health = GetComponent<Health>();
                int amount = health.CalculateMaxHealthPercentage(0.1f);
                health.HealDamage(amount);
            }

            if (!campfireSpawned) {
                campfireSpawned = true;

                Debug.Log("Spawning campfire...");

                Instantiate(campfireGO, campfirePlace.transform.position, Quaternion.identity);

                Destroy(campfirePlace);
            }
        }

        public override bool PrePerform() {
            if (agentBeliefs.HasState("isFighting")) {
                return false;
            }

            campfireSpawned = false;

            Vector3 pos = new Vector3(transform.position.x + Random.Range(-10f, 10f), 0,
                transform.position.z + Random.Range(-10f, 10f));

            campfirePlace = new GameObject("Campfire Position " + GetComponent<Entity>().entityName);
            campfirePlace.transform.position = pos;

            NavMeshPath path = new NavMeshPath();
            if (navMeshAgent.CalculatePath(campfirePlace.transform.position, path)) {
                destinationGO = campfirePlace;
            }
            else {
                Destroy(campfirePlace);
                return false;
            }

            return true;
        }
        public override bool PostPerform() {
            agentBeliefs.RemoveState(key: "isTired");

            return true;
        }

        public override bool IsAchievable() {
            if (agentBeliefs.HasState("isFighting")) {
                return false;
            }

            return true;
        }
    }
}

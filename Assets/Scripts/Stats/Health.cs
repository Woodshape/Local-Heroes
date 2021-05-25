using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LH.Stats {
    [RequireComponent(typeof(BaseStats))]
    public class Health : MonoBehaviour {
        public GameObject bloodPrefab;
        public Slider healthSlider;

        [SerializeField] float currentHealth = 1;

        private bool isDead = false;
        private float healthTick;
        private float updateTick;

        public delegate void deathDelegate(Health health);
        public event deathDelegate deathEvent;

        private void Start() {
            currentHealth = GetComponent<BaseStats>().GetStat(Stat.Health);

            GetComponent<BaseStats>().levelUpEvent += onLevelUp;
        }

        private void Update() {
            healthTick += Time.deltaTime;

            if (healthTick > 10f) {
                healthTick = 0f;
                
                HealDamage(1);
            }
        }

        public void TakeDamage(GameObject instigator, int amount) {
            int damage = Mathf.Abs(amount);

            currentHealth = Mathf.Max(currentHealth - damage, 0);

            Debug.Log($"{this.name} taking {damage} damage!");
            EvaluateHealth();

            if (currentHealth == 0) {
                StartCoroutine(Die());
                AwardExperience(instigator);
            }
        }

        public void HealDamage(int amount) {
            int heal = Mathf.Abs(amount);

            currentHealth = Mathf.Min(currentHealth + heal, GetComponent<BaseStats>().GetStat(Stat.Health));

            Debug.Log($"{this.name} healed for {heal}!");
            EvaluateHealth();
        }
        
        private void AwardExperience(GameObject instigator) {
            //  get our experience worth
            int amount = GetComponent<BaseStats>().GetExperienceReward();
            
            //  award it to our "instigator" - which could be dead
            if (instigator != null && instigator.GetComponent<Experience>() != null) {
                instigator.GetComponent<Experience>().GainExperience(amount);
            }
        }

        private void EvaluateHealth() {
            float percent = (float) currentHealth / GetComponent<BaseStats>().GetStat(Stat.Health);
            if (percent <= 0.5f) {
                GetComponent<Entity>().Beliefs.ModifyState("isHurt", 1);
            }
            else {
                GetComponent<Entity>().Beliefs.ModifyState("isHurt", -1);
            }

            UpdateHealthDisplay(percent);
        }

        private IEnumerator Die() {
            Debug.Log($"{this.name} dies!");

            isDead = true;

            //  invoke death event on all subscribers
            deathEvent?.Invoke(this);

            if (bloodPrefab) {
                Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
                Instantiate(bloodPrefab, pos, Quaternion.identity);
            }

            GetComponent<Entity>().RemoveSelfFromWorld();
            GetComponent<Entity>().RemoveSelfFromCombat();

            yield return new WaitForSeconds(.1f);

            Destroy(gameObject);
        }

        private void UpdateHealthDisplay(float percent) {
            if (healthSlider && !isDead) {
                healthSlider.value = percent;
            }
        }

        public bool IsDead() {
            return isDead;
        }

        private void onLevelUp() {
            int regen = Convert.ToInt32(GetComponent<BaseStats>().GetStat(Stat.Health) * 0.5f);
            HealDamage(regen);
        }
    }
}

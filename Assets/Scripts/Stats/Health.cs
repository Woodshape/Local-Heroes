using System;
using System.Collections;
using LH.Actions;
using LH.GOAP;
using UnityEngine;
using UnityEngine.UI;

namespace LH.Stats {
    [RequireComponent(typeof(BaseStats))]
    public class Health : MonoBehaviour {
        public GameObject bloodPrefab;
        public GameObject healPrefab;
        public Slider healthSlider;

        [SerializeField] float currentHealth = 1;

        private bool isDead = false;
        private float healthTick;
        private float updateTick;
        
        public event Action<Health> deathEvent;

        public event Action<float> damageEvent;

        public event Action healthChangedEvent;

        private void Start() {
            currentHealth = GetComponent<BaseStats>().GetStat(Stat.Health);

            GetComponent<BaseStats>().levelUpEvent += onLevelUp;
        }

        private void Update() {
            healthTick += Time.deltaTime;

            if (healthTick > 10f) {
                healthTick = 0f;

                HealDamage(1, false);
            }
        }

        public void TakeDamage(GameObject instigator, float amount) {
            float damage = Mathf.Abs(amount);
            
            damageEvent?.Invoke(damage);

            currentHealth = Mathf.Max(currentHealth - damage, 0);

            Debug.Log($"{this.name} taking {damage} damage!");
            EvaluateHealth();

            if (currentHealth == 0) {
                StartCoroutine(Die());
                AwardExperience(instigator);
                return;
            }
            
            //  interrupt current action if we were not wandering around
            if ((GetComponent<GAgent>().CurrentAction.IsInterruptable)) {
                GetComponent<GAgent>().CurrentAction.IsRunning = false;
            }
        }

        public void HealDamage(float amount, bool showEffect) {
            float heal = Mathf.Abs(amount);

            currentHealth = Mathf.Min(currentHealth + heal, GetComponent<BaseStats>().GetStat(Stat.Health));

            if (showEffect && healPrefab != null) {
                Instantiate(healPrefab, gameObject.transform);
            }

            Debug.Log($"{this.name} healed for {heal}!");
            EvaluateHealth();
        }
        
        public bool IsDead() {
            return isDead;
        }

        public float GetCurrentHealth() {
            return currentHealth;
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
            float percent = currentHealth / GetComponent<BaseStats>().GetStat(Stat.Health);
            if (percent <= 0.5f) {
                GetComponent<Entity>().Beliefs.ModifyState("isHurt", 1);
            }
            else {
                GetComponent<Entity>().Beliefs.ModifyState("isHurt", -1);
            }

            if (percent < 1.0f) {
                UpdateHealthDisplay(percent);
                healthChangedEvent?.Invoke();
            }
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

        private void onLevelUp() {
            int regen = Convert.ToInt32(GetComponent<BaseStats>().GetStat(Stat.Health) * 0.5f);
            HealDamage(regen, true);
        }
    }
}

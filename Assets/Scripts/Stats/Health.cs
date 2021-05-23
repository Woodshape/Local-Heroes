using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LH.Stats {
    [RequireComponent(typeof(BaseStats))]
    public class Health : MonoBehaviour {
        public GameObject bloodPrefab;
        public Slider healthSlider;

        private int maxHealth;
        [SerializeField] int currentHealth;

        private bool isDead = false;

        public delegate void deathDelegate(Health health);

        public event deathDelegate deathEvent;

        private void Start() {
            maxHealth = GetComponent<BaseStats>().GetHealth();
            currentHealth = maxHealth;
        }

        private void Update() {
            if (healthSlider && !isDead) {
                healthSlider.value = (float) currentHealth / maxHealth;
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

            currentHealth = Mathf.Min(currentHealth + heal, maxHealth);

            Debug.Log($"{this.name} healed for {heal}!");
            EvaluateHealth();
        }

        public int CalculateMaxHealthPercentage(float percent) {
            float per = Mathf.Clamp01(percent);
            return Convert.ToInt32(maxHealth * per);
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
            float percent = (float) currentHealth / maxHealth;
            Debug.Log($"Health at {percent}: {currentHealth} / {maxHealth}");
            if (percent <= 0.5f) {
                GetComponent<Entity>().Beliefs.ModifyState("isHurt", 1);
            }
            else {
                GetComponent<Entity>().Beliefs.ModifyState("isHurt", -1);
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

        public bool IsDead() {
            return isDead;
        }
    }
}

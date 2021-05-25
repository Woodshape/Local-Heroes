using System;
using LH.Stats;
using UnityEngine;

namespace LH.Data {
    public enum PotionType {
        Health,
        Buff,
        Hybrid
    }

    [CreateAssetMenu(fileName = "Potion", menuName = "LH/Items/Potions", order = 2)]
    public class Potion : Item, IConsumable {
        [Header("Potion")]
        public PotionType type;
        public float healAmount;
        public ItemModifier buffModifier;

        public bool Consume(Entity consumer) {
            bool success = false;

            Debug.Log("Trying top consume potion");
            
            switch (type) {
                case PotionType.Health:
                    Debug.Log("Potion: Health");
                    
                    consumer.GetComponent<Health>().HealDamage(healAmount, true);
                    success = true;
                    break;
                case PotionType.Buff:
                    //  TODO: implement buff class -> monobehaviour on player -> IStatModifier -> add this to buff class as modifier
                    success = true;
                    break;
            }

            return success;
        }
    }
}

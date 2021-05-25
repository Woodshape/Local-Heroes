using System.Collections;
using System.Collections.Generic;
using LH.Data;
using LH.GOAP;
using UnityEngine;

public class TakePotion : GAction {
    [SerializeField]
    public PotionType potion;

    public override bool IsAchievable() {
        if (!GetComponent<Inventory>().HasPotionOfType(potion)) {
            return false;
        }
        
        return true;
    }

    public override void Perform() {
    }
    
    public override bool PrePerform() {
        destinationGO = this.gameObject;
        return true;
    }
    
    public override bool PostPerform() {
        GetComponent<Inventory>().TakePotion(potion, GetComponent<Entity>());
        return true;
    }
}

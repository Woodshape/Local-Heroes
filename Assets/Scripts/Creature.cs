using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using LH.GOAP;
using UnityEngine;
using Random = UnityEngine.Random;

public class Creature : Entity
{
    protected override void Start() {
        base.Start();
        
        Debug.Log("Creating Creature...");
        
        OnSpawn();
        
        Goal findCharacterGoal = new Goal("findCharacter", 1, false);
        GoalsDict.Add(findCharacterGoal, 1);
        
        Goal wanderGoal = new Goal("wander", 1, false);
        GoalsDict.Add(wanderGoal, 3);

        Goal fightGoal = new Goal("fightCharacter", 3, false);
        GoalsDict.Add(fightGoal, 5);
    }

    public override void OnSpawn() {
        Debug.Log(gameObject.name + " spawned!");
        
        GWorld.Instance.GetQueue(Resource.CREATURE.ToString()).AddResource(this.gameObject, true);
        GWorld.Instance.GetWorldStates().ModifyState("creatures", 1);
    }
}

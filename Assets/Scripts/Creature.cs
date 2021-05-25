using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using LH.GOAP;
using LH.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

public class Creature : Entity, IStatModifier
{
    protected override void Start() {
        base.Start();
        
        Debug.Log("Creating Creature...");
        
        OnSpawn();
        
        //  FIXME
        int level = Random.Range(1, 11);
        GetComponent<BaseStats>().SetLevel(level);
        difficulty = (Difficulty)Random.Range(0, 6);

        Debug.Log($"Creature created of level {level} and difficulty {difficulty}");
        
        Goal findCharacterGoal = new Goal("findCharacter", 1, false);
        GoalsDict.Add(findCharacterGoal, 1);
        
        Goal wanderGoal = new Goal("wander", 1, false);
        GoalsDict.Add(wanderGoal, 3);

        Goal fightGoal = new Goal("fightCharacter", 3, false);
        GoalsDict.Add(fightGoal, 5);
    }

    public override void OnSpawn() {
        Debug.Log(gameObject.name + " spawned!");
        
        GWorld.Instance.GetQueue(Resource.Creature.ToString()).AddResource(this.gameObject, true);
        GWorld.Instance.GetWorldStates().ModifyState("creatures", 1);
    }
    
    public IEnumerable<float> GetModifier(Stat stat) {
        yield return 0f;
    }
    
    public IEnumerable<float> GetMultiplier(Stat stat) {
        float difficultyModifier = Difficulties.GetDifficultyModifier(difficulty);
        Debug.Log($"Adding {type} difficulty {difficulty} to {stat} multiplier: {difficultyModifier}!");
        yield return difficultyModifier;
    }
}

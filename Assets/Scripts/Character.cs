using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using LH.GOAP;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Character : Entity, IHumanoid {
    
    [Header("Humanoid")]
    [SerializeField]
    private GameObject weaponPrefab;
    [SerializeField]
    private Transform weaponHand;

    protected override void Start() {
        base.Start();

        Debug.Log("Starting Character");

        OnSpawn();

        Goal findEnemyGoal = new Goal("findCreature", 1, false);
        GoalsDict.Add(findEnemyGoal, 1);

        Goal fightGoal = new Goal("fightCreature", 1, false);
        GoalsDict.Add(fightGoal, 3);
        
        Goal restGoal = new Goal(state: "isRested", value: 1, remove: false);
        GoalsDict.Add(restGoal, 8);

        InvokeRepeating(nameof(GetTired), 0f, Random.Range(30f, 60f));
    }

    public override void OnSpawn() {
        Debug.Log(name + " spawned!");
        
        GWorld.Instance.GetQueue(Resource.CHARACTER.ToString()).AddResource(this.gameObject, true);
        GWorld.Instance.GetWorldStates().ModifyState("characters", 1);

        if (weaponPrefab != null) {
            Instantiate(weaponPrefab, weaponHand);
        }
    }

    private void GetTired() {
        GetComponent<GAgent>().Beliefs.ModifyState("isTired", 1);
    }
    
    public GameObject WeaponPrefab {
        get => weaponPrefab;
        set => weaponPrefab = value;
    }

    public Transform WeaponHand {
        get => weaponHand;
        set => weaponHand = value;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using LH.Data;
using LH.GOAP;
using LH.Stats;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Inventory))]
public class Character : Entity, IHumanoid, IStatModifier {
    
    [Header("Humanoid")]
    [SerializeField]
    private GameObject weaponPrefab;
    [SerializeField]
    private Transform weaponHand;
    [SerializeField]
    private Weapon currentWeapon;
    [SerializeField]
    private Inventory inventory;

    protected override void Start() {
        base.Start();

        Debug.Log("Starting Character");

        inventory = GetComponent<Inventory>();

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
        
        GWorld.Instance.GetQueue(Resource.Character.ToString()).AddResource(this.gameObject, true);
        GWorld.Instance.GetWorldStates().ModifyState("characters", 1);

        if (weaponPrefab != null) {
            GameObject weapon = Instantiate(weaponPrefab, weaponHand);

            currentWeapon = weapon.GetComponent<WeaponObject>().GetWeapon();
            inventory.EquipItem(currentWeapon);
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

    public Weapon CurrentWeapon {
        get => currentWeapon;
        set => currentWeapon = value;
    }
    
    public IEnumerable<float> GetModifier(Stat stat) {
        Debug.Log("Getting modifiers for stat: " + stat);
        
        float mod = 0f;
        foreach (Item item in inventory.GetEquippedItems()) {
            float itemMod = item.GetModifier(stat);
            if (itemMod != 0) {
                Debug.Log($"Adding mod on item {item}: {itemMod}");
                mod += itemMod;
            }
        }
        
        Debug.Log($"Adding {type} to {stat} modifier: {mod}!");
        
        yield return mod;
    }

    public IEnumerable<float> GetMultiplier(Stat stat) {
        Debug.Log("Getting multipliers for stat: " + stat);
        
        float multiplier = 0f;
        
        float difficultyModifier = Difficulties.GetDifficultyModifier(difficulty);
        Debug.Log($"Adding {type} difficulty {difficulty} to {stat} multiplier: {difficultyModifier}!");
        multiplier += difficultyModifier;

        foreach (Item item in inventory.GetEquippedItems()) {
            float itemMult = item.GetMultiplier(stat);
            if (itemMult != 0) {
                Debug.Log($"Adding mult on item {item}: {itemMult}");
                multiplier += itemMult;
            }
        }
        
        Debug.Log($"Adding {type} to {stat} multiplier: {multiplier}!");
        
        yield return multiplier;
    }
}

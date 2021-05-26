using System;
using System.Collections.Generic;
using DefaultNamespace;
using LH.Actions;
using LH.GOAP;
using LH.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Health))]
public abstract class Entity : GAgent {
    private static readonly int Attack = Animator.StringToHash("Attack");

    [Header("Basic")]
    public string entityName;
    public Resource type;
    public Difficulty difficulty;
    
    [Header("Animation")]
    public Animator animator;

    [Header("Stats")]
    public Health target;

    public BaseStats Stats;
    public int stamina = 100;
    public float attackSpeed = 1.5f;

    private int currentStamina;
    private float timeSinceLastAttack = 0f;
    private float timeSinceLastStamina = 0f;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        
        Debug.Log("Starting Entity");
        
        Goal aliveGoal = new Goal("stayAlive", 5, false);
        GoalsDict.Add(aliveGoal, 10);

        animator = GetComponent<Animator>();

        Stats = GetComponent<BaseStats>();
        
        entityName = type.ToString() + "_" + Random.Range(0, 1000000);
        this.gameObject.name = entityName;

        bool resource = Enum.TryParse(gameObject.tag, true, out Resource automatic);
        if (!resource) {
            Debug.LogError("Could not parse resource for tag: " + gameObject.tag);
            return;
        }

        type = automatic;
    }

    private void Update() {
        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastStamina += Time.deltaTime;

        if (timeSinceLastStamina > 1f) {
            timeSinceLastStamina = 0f;

            currentStamina = Mathf.Max(currentStamina + 10, stamina);
        }
    }

    public void FightWith(GameObject target) {
        FightAction[] actions = gameObject.GetComponents<FightAction>();
        if (actions == null || actions.Length == 0) {
            //  no fight actions found...
            Beliefs.RemoveState("isFighting");
            return;
        }

        //  we already have a target, so we better unsubscribe to its death event
        if (this.target != null) {
            this.target.deathEvent -= onTargetDeath;
        }

        //  set a new target
        this.target = target.GetComponent<Health>();

        //  subscribe to target's death event
        this.target.deathEvent += onTargetDeath;

        Debug.Log(this.name + " fighting " + target.name);
        Beliefs.ModifyState("isFighting", 1);

        foreach (FightAction action in actions) {
            action.destinationGO = target;
        }
    }

    public void RemoveSelfFromWorld() {
        GWorld.Instance.GetQueue(type.ToString()).RemoveResource(this.gameObject);

        //  "release" our target back to the world
        if (target) {
            GWorld.Instance.GetQueue(this.target.GetComponent<Entity>().type.ToString()).AddResource(target.gameObject, true);
        }

        if (GetComponent<WanderAction>()) {
            Destroy(GetComponent<WanderAction>().destinationGO);
        }

        switch (type) {
            case Resource.Character:
                GWorld.Instance.GetWorldStates().ModifyState("characters", -1);
                break;
            case Resource.Creature:
                GWorld.Instance.GetWorldStates().ModifyState("creatures", -1);
                break;
        }
    }

    public void RemoveSelfFromCombat() {
        if (this.target != null) {
            this.target.deathEvent -= onTargetDeath;
        }
        
        target = null;
        
        //  remove belief states
        Beliefs.RemoveState("isFighting");
        Beliefs.RemoveState("isAttacked");
    }

    public void AttackBehaviour(Animation attackAnimation) {
        if (target == null) {
            return;
        }
        
        if (!target.IsDead()) {
            if (timeSinceLastAttack > attackSpeed) {
                animator.SetTrigger(Attack);
                timeSinceLastAttack = 0;
            }
        }
    }

    public void RemoveStamina(int cost) {
        int amount = Mathf.Abs(cost);
        
        currentStamina = Mathf.Max(currentStamina - amount, 0);
    }

    public bool HasEnoughStamina(int cost) {
        return currentStamina >= cost;
    }

    private void onTargetDeath(Health h) {
        if (h == null || gameObject == null || GetComponents<FightAction>() == null) {
            return;
        }

        Debug.Log(h.gameObject.name + " death callback!!!");
        
        foreach (FightAction fightAction in GetComponents<FightAction>()) {
            fightAction.ClearTarget();
        }

        RemoveSelfFromCombat();
    }

    private void OnDestroy() {
        if (this.target != null) {
            this.target.deathEvent -= onTargetDeath;
        }
    }

    public abstract void OnSpawn();

    //  Animation Event
    void Hit() {
        if (target == null) {
            return;
        }
        
        target.TakeDamage(this.gameObject, Stats.GetStat(LH.Stats.Stat.Damage));
    }
}

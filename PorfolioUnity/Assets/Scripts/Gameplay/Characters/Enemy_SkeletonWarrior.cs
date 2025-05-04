using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy_SkeletonWarrior : Character
{
    private const string SKELETON_CHARGE = "SKELETON_CHARGE";
    
    public Player Victim { get; private set; }
    public EStateMachine<SkeletonWarriorState> StateMachine { get; private set; }

    private MovementAbility movementAbility;
    private HealthAbility healthAbility;
    private SpellsAbility spellsAbility;

    protected override void Create()
    {
        movementAbility = GetComponent<MovementAbility>();
        healthAbility = GetComponent<HealthAbility>();
        spellsAbility = GetComponent<SpellsAbility>();
    }

    protected override UniTask Init()
    {
        StateMachine = new EStateMachine<SkeletonWarriorState>(SkeletonWarriorState.Idle);
        spellsAbility.BindAbility<SkeletonWarrior_SkeletonCharge>(SKELETON_CHARGE);
        return base.Init();
    }

    protected override async UniTask PostInit()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1F));
        Victim = FindFirstObjectByType<Player>();
        StateMachine.ChangeState(Victim ? SkeletonWarriorState.Pursuing : SkeletonWarriorState.Idle);
    }

    protected override void Tick(float deltaTime)
    {
        if (!Victim || !StateMachine.IsState(SkeletonWarriorState.Pursuing))
        {
            return;
        }
        
        movementAbility.SetDestination(Victim.Position);
        
        var distance = Vector3.Distance(Position, Victim.Position);
        
        var isValidToCharge = distance is > 1F and < 3F;
        var isValidToAttack = distance < 0.25f;
        
        if (isValidToCharge)
        {
            Charge();
        }

        if (isValidToAttack)
        {
            Attack();
        }
    }

    private void Charge()
    {
        movementAbility.StopMovement();
        StateMachine.ChangeState(SkeletonWarriorState.Charging);
        spellsAbility.CastSpell(SKELETON_CHARGE);
    }

    private void Attack()
    {
        movementAbility.StopMovement();
        StateMachine.ChangeState(SkeletonWarriorState.Pursuing);
    }
    
    public override void Hit(Hit hit)
    {
        if (StateMachine.IsState(SkeletonWarriorState.Dead))
        {
            return;
        }
        
        var hitPoints = hit.InvokeHit();
        var dead = !healthAbility.UpdateHealth(hitPoints);

        if (dead)
        {
            Die();
        }
    }

    protected override void OnDie()
    {
        movementAbility.StopMovement();
        StateMachine.ChangeState(SkeletonWarriorState.Dead);
        gameObject.SetActive(false);
    }
}

public enum SkeletonWarriorState
{
    Idle,
    Pursuing,
    Attacking,
    Charging,
    Dead,
}

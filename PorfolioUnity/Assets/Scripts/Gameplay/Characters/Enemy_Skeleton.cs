using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy_Skeleton : Character, ISpellCaster
{
    private const string SKELETON_SACRIFICE = "SKELETON_SACRIFICE";
    
    public ISpellTarget Victim { get; private set; }

    
    private MovementAbility movementAbility;
    private HealthAbility healthAbility;
    private SpellsAbility spellsAbility;
    
    private EStateMachine<SkeletonState> stateMachine;
    
    protected override void Create()
    {
        movementAbility = GetComponent<MovementAbility>();
        healthAbility = GetComponent<HealthAbility>();
        spellsAbility = GetComponent<SpellsAbility>();
    }

    protected override UniTask Init()
    {
        stateMachine = new EStateMachine<SkeletonState>(SkeletonState.Idle);
        spellsAbility.BindAbility<Skeleton_Sacrifice>(SKELETON_SACRIFICE);
        return base.Init();
    }

    protected override async UniTask PostInit()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1F));
        Victim = FindFirstObjectByType<Player>();
        stateMachine.ChangeState(Victim != null ? SkeletonState.Pursuing : SkeletonState.Idle);
    }

    public override void OnGet()
    {
    }

    public override void OnRelease()
    {
        stateMachine.ChangeState(SkeletonState.Idle);
    }

    protected override void Tick(float deltaTime)
    {
        if (Victim == null || !stateMachine.IsState(SkeletonState.Pursuing))
        {
            return;
        }
        
        movementAbility.SetDestination(Victim.Transform.position);
        
        var inNear = Vector3.Distance(transform.position, Victim.Transform.position) < 0.25f;

        if (inNear)
        {
            Sacrifice();
        }
    }

    public override void Hit(Hit hit)
    {
        if (stateMachine.IsState(SkeletonState.Dead))
        {
            return;
        }
        
        var hitPoints = hit.InvokeHit();
        var isDamage = hit.hitType is HitType.DirectDamage or HitType.DotDamage;
        
        if (isDamage)
        {
            Sacrifice();
        }

        healthAbility.UpdateHealth(isDamage ? -9999F : hitPoints);
    }

    private void Sacrifice()
    {
        spellsAbility.CastSpell(SKELETON_SACRIFICE, 
            () => // success 
            {
                stateMachine.ChangeState(SkeletonState.Sacrificing);
                movementAbility.StopMovement();
            }, 
            () => // failure
            {
                stateMachine.ChangeState(SkeletonState.Pursuing);
            }
            , Die);
    }

    protected override void OnDie()
    {
        stateMachine.ChangeState(SkeletonState.Dead);
        gameObject.SetActive(false);
    }


    private enum SkeletonState
    {
        Idle,
        Pursuing,
        Sacrificing,
        Dead,
    }
    
    public SpellsAbility GetSpellsAbility()
    {
        return spellsAbility;
    }
}

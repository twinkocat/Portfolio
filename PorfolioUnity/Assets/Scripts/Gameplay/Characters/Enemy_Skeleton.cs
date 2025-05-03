using System;
using Cysharp.Threading.Tasks;

public class Enemy_Skeleton : Character
{
    private const string SKELETON_SACRIFICE = "SKELETON_SACRIFICE";
    
    private Player victim;
    
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
        victim = FindFirstObjectByType<Player>();
        stateMachine.ChangeState(victim ? SkeletonState.Pursuing : SkeletonState.Idle);
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
        if (!victim || !stateMachine.IsState(SkeletonState.Pursuing))
        {
            return;
        }
        
        movementAbility.SetDestination(victim.transform.position);
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
            movementAbility.StopMovement();
            stateMachine.ChangeState(SkeletonState.Sacrificing);
            spellsAbility.CastSpell(SKELETON_SACRIFICE);
        }

        healthAbility.UpdateHealth(isDamage ? -9999F : hitPoints);
    }

    public override void Die()
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
}

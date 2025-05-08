using System;
using Cysharp.Threading.Tasks;


public class Enemy_SkeletonWarrior : Character, ISpellCaster
{
    private const string SKELETON_CHARGE = "SKELETON_CHARGE";
    private const string SKELETON_SLASH_MELEE = "SKELETON_SLASH_MELEE";
    private const string SKELETON_SLASH_AFTER_CHARGE = "SKELETON_SLASH_AFTER_CHARGE";
    
    public ISpellTarget Victim { get; private set; }
    public EStateMachine<SkeletonWarriorState> StateMachine { get; private set; }

    private MovementComponent movementComponent;
    private HealthComponent healthComponent;
    private SpellsComponent spellsComponent;

    protected override void Create()
    {
        movementComponent = GetComponent<MovementComponent>();
        healthComponent = GetComponent<HealthComponent>();
        spellsComponent = GetComponent<SpellsComponent>();
    }

    protected override UniTask Init()
    {
        StateMachine = new EStateMachine<SkeletonWarriorState>(SkeletonWarriorState.Idle);
        spellsComponent.BindAbility<SkeletonWarrior_Charge>(SKELETON_CHARGE);
        spellsComponent.BindAbility<SkeletonWarrior_SlashMelee>(SKELETON_SLASH_MELEE);
        spellsComponent.BindAbility<SkeletonWarrior_SlashAfterCharge>(SKELETON_SLASH_AFTER_CHARGE);
        return base.Init();
    }

    protected override async UniTask PostInit()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1F));
        Victim = FindFirstObjectByType<Player>();
        StateMachine.ChangeState(Victim != null ? SkeletonWarriorState.Pursuing : SkeletonWarriorState.Idle);
    }

    protected override void Tick(float deltaTime)
    {
        if (Victim == null || !StateMachine.IsState(SkeletonWarriorState.Pursuing))
        {
            return;
        }
        
        movementComponent.SetDestination(Victim.Transform.position);
        TryCharge();
    }

    private void TryCharge()
    {
        spellsComponent.CastSpell(SKELETON_CHARGE, Charge, () =>
        {
            Pursuing();
            TryMeleeAttack();
        }, AttackAfterCharge);
    }

    private void AttackAfterCharge()
    {
        spellsComponent.CastSpell(SKELETON_SLASH_AFTER_CHARGE, Attack, Pursuing, TryMeleeAttack);
    }

    private void Charge()
    {
        StateMachine.ChangeState(SkeletonWarriorState.Charging);
        movementComponent.StopMovement();
    }

    private void TryMeleeAttack()
    {
        spellsComponent.CastSpell(SKELETON_SLASH_MELEE, Attack, Pursuing, TryMeleeAttack);
    }

    private void Attack()
    {
        movementComponent.StopMovement();
        StateMachine.ChangeState(SkeletonWarriorState.Attacking);
    }

    private void Pursuing()
    {
        StateMachine.ChangeState(SkeletonWarriorState.Pursuing);
    }
    
    public override void Hit(Hit hit)
    {
        if (StateMachine.IsState(SkeletonWarriorState.Dead))
        {
            return;
        }
        
        var hitPoints = hit.InvokeHit();
        var dead = !healthComponent.UpdateHealth(hitPoints);

        if (dead)
        {
            Die();
        }
    }

    protected override void OnDie()
    {
        movementComponent.StopMovement();
        StateMachine.ChangeState(SkeletonWarriorState.Dead);
        gameObject.SetActive(false);
    }

    public SpellsComponent GetSpellComponent()
    {
        return spellsComponent;
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

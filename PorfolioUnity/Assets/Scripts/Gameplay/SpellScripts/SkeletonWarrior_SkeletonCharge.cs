using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SkeletonWarrior_SkeletonCharge : SpellScript
{
    private static readonly int IdleTrigger = Animator.StringToHash("Idle");
    private static readonly int ChargeTrigger = Animator.StringToHash("Charge");

    private ISpellTarget victim;
    private Enemy_SkeletonWarrior owner;
    private Animator animator;
    
    private const TargetFlags DAMAGEABLE_FLAGS = TargetFlags.Player;
    
    private const float CHARGE_TIME = 0.5F;
    private const float RADIUS = 2.5F;

    protected override void PreExecute()
    {
        owner = GetOwner<Enemy_SkeletonWarrior>(true);
        victim = owner.Victim;
        animator = GetAnimator();
    }

    protected override async UniTask ExecuteSpellAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(0.5F), cancellationToken);
        animator.SetTrigger(ChargeTrigger);
        await Charge(cancellationToken);
        animator.SetTrigger(IdleTrigger);
        await Task.Delay(TimeSpan.FromSeconds(0.5F), cancellationToken);
        
        DebugShape.CreateCircle(owner.Position, RADIUS, 1F, Color.red);
        await ExecuteCircleSpell(owner.Position, RADIUS, DAMAGEABLE_FLAGS);
        
        owner.StateMachine.ChangeState(SkeletonWarriorState.Pursuing);
    }

    private async UniTask Charge(CancellationToken cancellationToken)
    {
        var startPos = owner.Position;
        var endPos = victim.Position;
        
        DebugShape.CreateCircle(endPos, 0.3F, CHARGE_TIME, Color.blue);
        
        for (var time = 0F; time < CHARGE_TIME; time += Time.deltaTime)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            
            var progress = Mathf.Clamp01(time / CHARGE_TIME);
            owner.transform.position = Vector3.Lerp(startPos, endPos, progress);
            await UniTask.NextFrame();
        }
    }

    protected override void OnHit(ISpellTarget target)
    {
        target.Hit(new Hit
        {
            hitPosition = target.Position,
            hitType = HitType.DirectDamage,
            rawHit = 10,
        });   
    }
}

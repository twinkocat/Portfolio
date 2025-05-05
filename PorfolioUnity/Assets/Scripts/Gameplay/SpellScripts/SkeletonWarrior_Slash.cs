using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SkeletonWarrior_Slash : SpellScript
{
    private Vector3 ownerPosition;
    private Vector3 ownerForward;
    
    protected override void PreExecute()
    {
        var owner = GetOwner();
        ownerPosition = owner.Position;
        ownerForward = owner.transform.forward;
    }

    protected override async UniTask ExecuteSpellAsync(CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5F), cancellationToken: cancellationToken);
        DebugShape.CreateCone(ownerPosition, ownerForward, 75F, 1F,  0.5F, Color.red);
        await ExecuteConeSpell(ownerPosition, ownerForward, 1F, 75F, TargetFlags.Player);
        GetOwner<Enemy_SkeletonWarrior>().StateMachine.ChangeState(SkeletonWarriorState.Pursuing);
    }

    protected override void OnHit(ISpellTarget target)
    {
        target.Hit(new Hit
        {
            rawHit = 20,
            hitType = HitType.DirectDamage,
            hitPosition = target.Position,
        });
    }
}

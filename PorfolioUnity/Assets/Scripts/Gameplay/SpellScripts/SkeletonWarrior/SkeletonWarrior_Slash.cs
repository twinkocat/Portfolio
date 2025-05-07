using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class SkeletonWarrior_SlashAfterCharge : ConeSpellScript<SkeletonWarrior_SlashData>
{
    protected override async UniTask Activate(CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(data.castTime), cancellationToken: cancellationToken);
        
        var owner = GetOwner();
        var position = owner.Transform.position;
        var forward = owner.Transform.forward;
        ConeSpell_Internal(position, forward);
        await UniTask.Delay(TimeSpan.FromSeconds(data.posCastTime), cancellationToken: cancellationToken);
    }

    protected override void OnHit(ISpellTarget target)
    {
        target.Hit(new Hit
        {
            hitPosition = target.Transform.position,
            rawHit = data.damage,
            hitType = HitType.DirectDamage
        });
    }
}



public class SkeletonWarrior_SlashMelee : SkeletonWarrior_SlashAfterCharge
{
    protected override bool CanCast()
    {
        if (!base.CanCast())
        {
            return false;
        }
        
        var owner = GetOwner();
        var victim = owner.Victim;

        if (victim == null)
        {
            Debug.LogWarning("No victim found!");
            return false;
        }
        
        var position = owner.Transform.position;
        var victimPosition = victim.Transform.position;

        return Vector3.Distance(position,  victimPosition) < data.validDistance;
    }
}



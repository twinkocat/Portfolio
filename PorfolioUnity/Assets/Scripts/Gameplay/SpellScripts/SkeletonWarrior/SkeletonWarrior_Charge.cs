using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SkeletonWarrior_Charge : ChargeSpellScript<SkeletonWarrior_ChargeData>
{
    protected override async UniTask Activate(CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(data.castTime), cancellationToken: cancellationToken);
        var endPosition = GetOwner().Victim.Transform.position;
        await Charge_Internal(cancellationToken, endPosition);
    }

    protected override void OnHit(ISpellTarget target)
    {
    }

    protected override bool CanCast()
    {
        if (!base.CanCast())
        {
            return false;
        }
        
        var victim = GetOwner().Victim;
        
        if (victim == null)
        {
            Debug.LogWarning("No victim assigned to SkeletonWarrior");
            return false;
        }
        
        var distance = Vector3.Distance(victim.Transform.position, GetOwner().Transform.position);
        return distance > data.chargeRange.Min && distance < data.chargeRange.Max;
    }
}



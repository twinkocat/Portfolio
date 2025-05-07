using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Skeleton_Sacrifice : CircleSpellScript<Skeleton_SacrificeData>
{
    private int maxHealTargets = 0;
    private int healedTargets = 0;


    protected override async UniTask Activate(CancellationToken cancellationToken)
    {
        maxHealTargets = data.maxHealTargets.GetValueRoundToInt();
        
        await UniTask.Delay(TimeSpan.FromSeconds(data.castTime), cancellationToken: cancellationToken);
        
        var owner = GetOwner();
        var position = owner.Transform.position;
        
        CircleSpell_Internal(position);
    }

    protected override void OnHit(ISpellTarget target)
    {
        var owner = GetOwner<ISpellTarget>(true);

        if (owner.Equals(target))
        {
            return;
        }
        
        if (healedTargets <= maxHealTargets && target.Flags.HasFlag(TargetFlags.Enemy))
        {
            target.Hit(new Hit
            {
                rawHit = data.damage,
                hitPosition = target.Transform.position,
                hitType = HitType.DirectHeal,
            });
            healedTargets++;
        }
        
        if (target.Flags.HasFlag(TargetFlags.Player))
        {
            target.Hit(new Hit
            {
                rawHit = data.heal,
                hitPosition = target.Transform.position,
                hitType = HitType.DirectDamage,
        
            });
        }
    }
}
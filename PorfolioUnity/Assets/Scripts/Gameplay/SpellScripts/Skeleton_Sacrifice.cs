using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Skeleton_Sacrifice : SpellScript
{
    private const float AOE_RANGE = 1.5F;
    private const TargetFlags SPELL_FLAG = TargetFlags.Player & TargetFlags.Enemy;
    private int maxHealTargets = 0;
    private int healTargets = 0;
    
    protected override async UniTask ExecuteSpellAsync(CancellationToken cancellationToken)
    {
        maxHealTargets = UnityEngine.Random.Range(1, 4);
        
        await UniTask.Delay(TimeSpan.FromSeconds(1F), cancellationToken: cancellationToken);

        var owner = GetOwner();
        var position = owner.transform.position;
        
        DebugShape.CreateCircle(position, AOE_RANGE, 1F, Color.red);
        await ExecuteCircleSpell(position, AOE_RANGE,SPELL_FLAG);
        
        owner.Die();
    }

    protected override void OnHit(ISpellTarget target)
    {
        var owner = GetOwner();
        
        if (owner.Equals(target))
        {
            return;
        }

        if (target.Flags.HasFlag(TargetFlags.Player))
        {
            target.Hit(new Hit
            {
                rawHit = CalculateDamage(),
                hitPosition = target.Position,
                hitType = HitType.DirectDamage,
                
            });
        }
        
        if (healTargets <= maxHealTargets && target.Flags.HasFlag(TargetFlags.Enemy))
        {
            target.Hit(new Hit
            {
                rawHit = CalculateHealing(),
                hitPosition = target.Position,
                hitType = HitType.DirectHeal,
            });
        }
        healTargets++;
    }

    private float CalculateHealing()
    {
        return 5F;
    }

    private float CalculateDamage()
    {
        return 10F;
    }
}

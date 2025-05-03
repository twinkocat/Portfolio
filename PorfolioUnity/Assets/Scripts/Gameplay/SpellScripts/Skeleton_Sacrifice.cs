using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class Skeleton_Sacrifice : SpellScript
{
    private const float AOE_RANGE = 1.5F;
    private const TargetFlags SPELL_FLAG = TargetFlags.Player & TargetFlags.Enemy;
    
    protected override async UniTask ExecuteSpellAsync(CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1F), cancellationToken: cancellationToken);

        var owner = GetOwner();
        var position = owner.transform.position;
        
        DebugShape.CreateCircle(position, AOE_RANGE, 1F);
        await ExecuteCircleSpell(position, AOE_RANGE,SPELL_FLAG);
    }

    protected override void OnHit(ISpellTarget target)
    {
        var owner = GetOwner();
        
        if (owner.Equals(target))
        {
            return;
        }
        
        target.Hit(new Hit
        {
            rawHit = target.Flags.HasFlag(TargetFlags.Player) ? CalculateDamage() : CalculateHealing(),
            hitPosition = target.Position,
            hitType = target.Flags.HasFlag(TargetFlags.Player) ? HitType.DirectDamage : HitType.DirectHeal,
        });
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

using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Warrior_SlashSpellScript : PlayerSpellScript
{
    private const TargetFlags DAMAGEABLE_FLAGS = TargetFlags.Enemy;
    private const float CONE_ANGLE = 45F;
    private const float CONE_LENGTH = 2F;
    private const float COOLDOWN = 0.75F;
    
    protected override async UniTask ExecuteSpellAsync(CancellationToken cancellationToken)
    {
        SetCooldown(COOLDOWN);
        
        var owner = GetOwner();
        var position = owner.transform.position;
        var direction = owner.transform.forward;
        DebugShape.CreateCone(position, direction, CONE_ANGLE, CONE_LENGTH, 1F, Color.cyan);
        
        await ExecuteConeSpell(position, direction, CONE_LENGTH, CONE_ANGLE, DAMAGEABLE_FLAGS);
    }

    protected override void OnHit(ISpellTarget target)
    {
        var damage = CalculateDamage(1F, 10F);

        target.Hit(new Hit
        {
            rawHit = damage,
            hitType = HitType.DirectDamage,
            hitPosition = target.Position,
        });
    }

    private float CalculateDamage(float levelMod, float attackPower)
    {
        return 5F * levelMod + attackPower;
    }

    protected override PlayerSpellType SpellViewIndex => PlayerSpellType.Primary0;
}
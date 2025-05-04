using System.Threading;
using Cysharp.Threading.Tasks;

public class Warrior_EarthShatterSpellScript : PlayerSpellScript
{
    private const TargetFlags DAMAGEABLE_FLAGS = TargetFlags.Enemy;
    private const float RADIUS = 4F;
    private const float COOLDOWN = 5F;
    
    protected override async UniTask ExecuteSpellAsync(CancellationToken cancellationToken)
    {
        SetCooldown(COOLDOWN);
        
        var owner = GetOwner();
        var position = owner.transform.position;
        DebugShape.CreateCircle(position, RADIUS, 1F);
        await ExecuteCircleSpell(position, RADIUS, DAMAGEABLE_FLAGS);
    }

    protected override void OnHit(ISpellTarget target)
    {
        var damage = CalculateDamage(1, 10);
        
        target.Hit(new Hit()
        {
            rawHit = damage,
            hitType = HitType.DirectDamage,
            hitPosition = target.Position,
        });
    }

    private float CalculateDamage(float levelMod, float attackPower)
    {
        return 2 * attackPower + 3 * levelMod;
    }

    protected override PlayerSpellType SpellViewIndex => PlayerSpellType.Ultimate;
}
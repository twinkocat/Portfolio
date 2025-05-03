using System;
using Cysharp.Threading.Tasks;

public class Warrior_SmashSpellScript : SpellScript
{
    private const TargetFlags DAMAGEABLE_FLAGS = TargetFlags.Enemy;
    private const float RADIUS = 2F;
    
    protected override async UniTask ExecuteSpellAsync()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1)); // Animation Time
     
        var owner = GetOwner();
        var position = owner.transform.position;
        
        await ExecuteCircleSpell(position, RADIUS, DAMAGEABLE_FLAGS);
    }

    protected override void OnHit(ISpellTarget target)
    {
        var damage = CalculateDamage(1, 10);

        target.Hit(new Hit()
        {
            rawDamage = damage,
        });
    }

    private float CalculateDamage(float levelMod, float attackPower)
    {
        return 2 * attackPower + 3 * levelMod;
    }
}
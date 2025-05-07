using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Warrior_EarthShatter : CircleSpellScript<Warrior_EarthShatterData>
{
    protected override async UniTask Activate(CancellationToken cancellationToken)
    {
        await UniTask.Yield();
        
        var position = GetOwner().Transform.position;
        CircleSpell_Internal(position);
    }

    protected override void OnHit(ISpellTarget target)
    {
        var spellAbility = GetOwnerAbility();
        var damage = spellAbility.AttackPower.Value * 3 * (spellAbility.LevelMod.Value * 2F);
 
        target.Hit(new Hit()
        {
            rawHit = damage,
            hitType = HitType.DirectDamage,
            hitPosition = target.Transform.position,
        });
    }
}

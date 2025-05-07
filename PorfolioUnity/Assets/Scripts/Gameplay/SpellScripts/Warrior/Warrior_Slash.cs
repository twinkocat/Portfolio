using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Warrior_Slash : ConeSpellScript<Warrior_SlashData>
{
    protected override async UniTask Activate(CancellationToken cancellationToken)
    {
        await UniTask.Yield();

        var owner = GetOwner();
        var position = owner.Transform.position;
        var forward = owner.Transform.forward;
        
        ConeSpell_Internal(position, forward);
    }

    protected override void OnHit(ISpellTarget target)
    {
        var ownerAbility = GetOwnerAbility();
        var damage = 5F * ownerAbility.LevelMod.Value + ownerAbility.AttackPower.Value;
        
        target.Hit(new Hit
        {
            rawHit = damage,
            hitType = HitType.DirectDamage,
            hitPosition = target.Transform.position,
        });
    }
}
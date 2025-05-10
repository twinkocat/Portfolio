using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Warrior_Smash : CircleSpellScript<Warrior_SmashData>
{
    protected override async UniTask Activate(CancellationToken cancellationToken)
    {
        await UniTask.Yield();
        var position = GetOwner().Transform.position;
        CircleSpell_Internal(position);
    }
    
    protected override void OnHit(ISpellTarget target)
    {
        var ownerAbility = GetOwnerSpellsComponent();
        var damage = 2F * ownerAbility.AttackPower.Value * ownerAbility.LevelMod.Value;
        
        target.Hit(new Hit()
        {
            rawHit = damage,
            hitType = HitType.DirectDamage,
            hitPosition = target.Transform.position,
        });
    }

    public override void OnPlayerSet()
    {
        Player.OnSpellSet?.Invoke(1, data);
    }
    
    protected override void OnCooldownTick(TimerData timerData)
    {
        Player.OnCooldownUpdated?.Invoke(1, 1 - timerData.GetNormalized());
    }

    protected override void OnCooldownComplete()
    {
        Player.OnCooldownUpdated?.Invoke(1, 0);
    }
}

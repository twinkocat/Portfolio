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
    
    public override void OnPlayerSet()
    {
        Player.OnSpellSet?.Invoke(2, data);
    }

    protected override void OnHit(ISpellTarget target)
    {
        var spellAbility = GetOwnerSpellsComponent();
        var damage = spellAbility.AttackPower.Value * 3 * (spellAbility.LevelMod.Value * 2F);
 
        target.Hit(new Hit()
        {
            rawHit = damage,
            hitType = HitType.DirectDamage,
            hitPosition = target.Transform.position,
        });
    }
    
    protected override void OnCooldownTick(TimerData timerData)
    {
        Player.OnCooldownUpdated?.Invoke(2, 1 - timerData.GetNormalized());
    }

    protected override void OnCooldownComplete()
    {
        Player.OnCooldownUpdated?.Invoke(2, 0);
    }
}

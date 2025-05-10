using System.Threading;
using Cysharp.Threading.Tasks;


public class Warrior_Charge : ChargeSpellScript<Warrior_ChargeData>
{
    protected override async UniTask Activate(CancellationToken cancellationToken)
    {
        var owner = GetOwner();
        var endPos = owner.Transform.position + owner.Transform.forward * data.chargeLenght; 
        await Charge_Internal(cancellationToken, endPos);
    }

    protected override void OnHit(ISpellTarget target)
    {
    }
    
    public override void OnPlayerSet()
    {
        Player.OnSpellSet?.Invoke(3, data);
    }
    
    protected override void OnCooldownTick(TimerData timerData)
    {
        Player.OnCooldownUpdated?.Invoke(3, 1 - timerData.GetNormalized());
    }

    protected override void OnCooldownComplete()
    {
        Player.OnCooldownUpdated?.Invoke(3, 0);
    }
}
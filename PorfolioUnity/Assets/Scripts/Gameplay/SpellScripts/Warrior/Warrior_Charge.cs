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
}
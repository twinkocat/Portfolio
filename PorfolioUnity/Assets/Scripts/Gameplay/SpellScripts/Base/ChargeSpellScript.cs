

using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class ChargeSpellScript<TData> : SpellScript<TData> where TData : ChargeSpellData
{
    protected async UniTask Charge_Internal(CancellationToken cancellationToken, Vector3 target)
    {
        var owner = GetOwner();
        var startPos = owner.Transform.position;
        for (var time = 0F; time < data.chargeTime; time += Time.deltaTime)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            var progress = Mathf.Clamp01(time / data.chargeTime);
            var t = data.chargeCurve.Evaluate(progress);
            
            owner.Transform.position = Vector3.Lerp(startPos, target, t);
            await UniTask.NextFrame();
        }
    }
}

public abstract class ChargeSpellData : SpellData
{
    public float chargeTime;
    public AnimationCurve chargeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
}

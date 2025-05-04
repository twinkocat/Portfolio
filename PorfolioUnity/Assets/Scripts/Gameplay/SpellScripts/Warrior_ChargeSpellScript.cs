using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Warrior_ChargeSpellScript : SpellScript
{
    private static readonly int DashTrigger = Animator.StringToHash("Dash");

    private const float COOLDOWN = 2F;
    private const float CHARGE_TIME = 0.5F;
    private const float DASH_SPEED = 5F;
    
    protected override async UniTask ExecuteSpellAsync(CancellationToken cancellationToken)
    {
        SetCooldown(COOLDOWN);
        GetAnimator().SetTrigger(DashTrigger);
        await Charge(cancellationToken);
    }

    private async UniTask Charge(CancellationToken cancellationToken)
    {
        var owner = GetOwner();
        var curve = AnimationCurve.Linear(0F, 0F, 1F, 1F);
        for (var time = 0F; time < CHARGE_TIME; time += Time.deltaTime)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            var normalizedTime = Mathf.Clamp01(time / CHARGE_TIME);
            var curveValue = curve.Evaluate(normalizedTime);
            owner.transform.position += owner.transform.forward * (DASH_SPEED * curveValue * Time.deltaTime);
            await UniTask.NextFrame();
        }
    }
}

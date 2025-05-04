using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Warrior_ChargeSpellScript : PlayerSpellScript
{
    private static readonly int DashTrigger = Animator.StringToHash("Dash");

    private const float COOLDOWN = 2F;
    private const float CHARGE_TIME = 0.5F;
    private const float DASH_SPEED = 5F;

    protected override async UniTask ExecuteSpellAsync(CancellationToken cancellationToken)
    {
        GetAnimator().SetTrigger(DashTrigger);
        SetCooldown(COOLDOWN);
        await Charge(cancellationToken);
    }

    private async UniTask Charge(CancellationToken cancellationToken)
    {
        var owner = GetOwner();
        for (var time = 0F; time < CHARGE_TIME; time += Time.deltaTime)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            owner.transform.position += owner.transform.forward * (DASH_SPEED * Time.deltaTime);
            await UniTask.NextFrame();
        }
    }

    protected override PlayerSpellType SpellViewIndex => PlayerSpellType.Dash;
}

using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

public class Player_Respawn : SpellScript
{
    [Inject] private HudMediator hudMediator;
    
    protected override UniTask Activate(CancellationToken cancellationToken)
    {
        hudMediator.Show();
        return UniTask.CompletedTask;
    }

    protected override void OnHit(ISpellTarget target)
    {
    }
}

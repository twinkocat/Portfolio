using System.Threading;
using Cysharp.Threading.Tasks;

public class InitializationState : IGameState
{
    public async UniTask StartAsync(CancellationToken cts)
    {
        await UniTask.Yield();
        
        Game.SetState<PlayingState>();
    }


    public void Dispose()
    {
        
    }
}
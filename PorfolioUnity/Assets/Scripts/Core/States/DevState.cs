using System.Threading;
using Cysharp.Threading.Tasks;

public class DevState : IGameState
{
    public UniTask StartAsync(CancellationToken cts)
    {
        return UniTask.CompletedTask;
    }


    public void Dispose()
    {
        
    }
}

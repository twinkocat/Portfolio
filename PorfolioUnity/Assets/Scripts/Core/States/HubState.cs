using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class HubState : IGameState, IProgress<float>
{
    public async UniTask StartAsync(CancellationToken cts)
    {
        await SceneManager.LoadSceneAsync((int)Scene.Hub, LoadSceneMode.Single).ToUniTask(this, cancellationToken: cts);
        await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cts);
    }


    public void Dispose()
    {
        
    }

    public void Report(float value)
    {
    }
}
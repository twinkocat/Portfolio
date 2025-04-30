using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LevelState : IGameState, IProgress<float>
{
    public async UniTask StartAsync(CancellationToken cts)
    {
        await SceneManager.LoadSceneAsync((int)Scene.Level, LoadSceneMode.Single).ToUniTask(this, cancellationToken: cts);
        
    }

    public void Dispose()
    {
        
    }

    public void Report(float value)
    {
    }
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializationState : IGameState, IProgress<float>
{
    public async UniTask StartAsync(CancellationToken cts)
    {
        await UniTask.Yield();
        await ResolveCurrentScene(cts);
        Game.SetState<HubState>();
    }

    private async UniTask ResolveCurrentScene(CancellationToken cts)
    {
        const int mainSceneIndex = (int)Scene.Main;
        var currentScene = SceneManager.GetActiveScene();
        
        if (currentScene.buildIndex != mainSceneIndex)
        {
            Debug.Log($"Wrong scene: {currentScene.name}. Loading main scene...");
            await SceneManager.LoadSceneAsync(mainSceneIndex, LoadSceneMode.Single).ToUniTask(this, cancellationToken: cts);
        }
        
        await UniTask.Yield();
    }
    
    public void Report(float value)
    {
    }

    public void Dispose()
    {
        
    }

}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class GameStateController : IDisposable
{
    private readonly CancellationTokenSource cts = new();
    private UniTask currentTask = UniTask.CompletedTask;
    private IGameState current;

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    private static void Init()
    {
        UnityEditor.EditorApplication.playModeStateChanged += change =>
        {
            if (change == UnityEditor.PlayModeStateChange.ExitingEditMode)
            {
                _wantsToQuit = false;
            }
            else if (change == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                _wantsToQuit = true;
            }
        };
    }

    private static bool _wantsToQuit = false;
#endif   
    
    public void LoadState<T>() where T : IGameState
    {
        var loadStateMessage = current != null ? current.GetType().Name : "No State";
        Debug.Log($"State transition: from [{loadStateMessage}] to [{typeof(T).Name}]");
        
#if UNITY_EDITOR
        if (_wantsToQuit)
        {
            return;
        }
#endif
        var completed = currentTask.Status is not UniTaskStatus.Pending;

        if (completed)
        {
            current?.Dispose();
            current = Game.Resolver.Resolve<T>();
            current.Start();
            RunAsyncTask(current);
            
        }
        else
        {
            Debug.LogWarning($"Trying to load state {typeof(T).Name}, before previous is loaded.");
            UniTask.Create(async () =>
            {
                await currentTask;
                current?.Dispose();
                current = Game.Resolver.Resolve<T>();
                current.Start();
                RunAsyncTask(current);
            });
        }
    }

    private void RunAsyncTask(IGameState state)
    {
        try
        {
            currentTask = state.StartAsync(cts.Token);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        }
    }
    
    public void Dispose()
    {
        cts.Cancel();
        cts.Dispose();
        current.Dispose();
        Debug.Log("StateController disposed");
    }
}

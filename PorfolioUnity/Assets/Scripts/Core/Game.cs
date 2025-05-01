using VContainer;
using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Game : IDisposable
{
    private static Game _instance;
    private readonly GameStateController stateController;
    
    public static IObjectResolver Resolver { get; private set; }
    
    public static Quaternion IsometricMod { get; } = Quaternion.AngleAxis(-45F, Vector3.up);
    
    public Game(IObjectResolver resolver)
    {
        _instance = this;
        stateController = new GameStateController();
        Resolver = resolver;
    }
    
    public static void SetState<T>() where T : IGameState
    {
        _instance.stateController.LoadState<T>();
    }
    
    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }

    public void Dispose()
    {
        stateController.Dispose();
        _instance = null;
    }
}

public interface IGameState : IDisposable
{
    void Start() { }

    UniTask StartAsync(CancellationToken cts)
    {
        return UniTask.CompletedTask;
    }
}

public enum Scene
{
    Main = 0,
    Hub = 1,
    Level = 2,
}
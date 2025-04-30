using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private Player playerPrefab;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(playerPrefab, Lifetime.Transient);
        builder.Register<Game>(Lifetime.Singleton);
        builder.Register<InitializationState>(Lifetime.Transient).AsSelf().As<InitializationState>();
        builder.Register<HubState>(Lifetime.Transient).AsSelf().As<HubState>();
        builder.Register<LevelState>(Lifetime.Transient).AsSelf().As<LevelState>();
    }
}

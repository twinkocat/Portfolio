using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<Game>(Lifetime.Singleton);
        builder.Register<InitializationState>(Lifetime.Transient).AsSelf().As<InitializationState>();
        builder.Register<PlayingState>(Lifetime.Transient).AsSelf().As<PlayingState>();
    }
}

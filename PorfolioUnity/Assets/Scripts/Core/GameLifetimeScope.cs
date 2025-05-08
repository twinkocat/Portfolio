using MyBox;
using UnityEngine;
using VContainer;
using VContainer.Unity;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif


public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Enemy_Skeleton skeletonPrefab;
    [SerializeField] private Enemy_SkeletonWarrior skeletonWarriorPrefab;
    
    [SerializeField] private View[] views;
    [SerializeField] private UIRoot uiRoot;
    
    private readonly AuraScriptsConfigurator auraScriptsConfigurator = new(); 
    private readonly WorldStateConfigurator worldStateConfigurator = new(); 
    private readonly MediatorsConfigurator mediatorsConfigurator = new();
    private readonly SpellScriptsConfigurator spellScriptsConfigurator = new();
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(playerPrefab, Lifetime.Transient);
        builder.RegisterComponentInNewPrefab(skeletonPrefab, Lifetime.Transient);
        builder.RegisterComponentInNewPrefab(skeletonWarriorPrefab, Lifetime.Transient);
        
        builder.Register<Game>(Lifetime.Singleton);
        builder.Register<GameTime>(Lifetime.Singleton);

        builder.RegisterComponent(uiRoot);
        views.ForEach(view => builder.RegisterComponent(view).AsSelf());
        worldStateConfigurator.Configure(builder);
        mediatorsConfigurator.Configure(builder);
        spellScriptsConfigurator.Configure(builder);
        auraScriptsConfigurator.Configure(builder);
        Resources.LoadAll<SpellData>("SpellData").ForEach(data => builder.RegisterInstance(data).AsSelf());
        Resources.LoadAll<AuraData>("AuraData").ForEach(data => builder.RegisterInstance(data).AsSelf());
    }
    
    
#if UNITY_EDITOR
    
    [DidReloadScripts] [MenuItem("Portfolio/Force Generate Lifetime Code")]
    public static void OnReloadScripts()
    {
        LifetimeCodeGenerator.GenerateRegistrationCode<IGameState, WorldStateConfigurator>
        (
            registrationLineGenerator: type => $"builder.Register<{type.FullName}>(Lifetime.Transient)" +
                                               $".AsSelf()" +
                                               $".As<{typeof(IGameState).FullName}>();"
        );
        
        LifetimeCodeGenerator.GenerateRegistrationCode<Mediator, MediatorsConfigurator>
        (
            registrationLineGenerator: type => $"builder.Register<{type.FullName}>(Lifetime.Singleton)" +
                                               $".AsSelf()" +
                                               $".AsImplementedInterfaces();"
        );
        
        LifetimeCodeGenerator.GenerateRegistrationCode<SpellScript, SpellScriptsConfigurator>
        (
            registrationLineGenerator: type => $"builder.Register<{type.FullName}>(Lifetime.Transient);"
        );
        
        LifetimeCodeGenerator.GenerateRegistrationCode<AuraScript, AuraScriptsConfigurator>
        (
            registrationLineGenerator: type => $"builder.Register<{type.FullName}>(Lifetime.Transient);"
        );
    }
#endif
}



public partial class MediatorsConfigurator
{
    public void Configure(IContainerBuilder builder) => Configure_Internal(builder);
    partial void Configure_Internal(IContainerBuilder builder);
}

public partial class WorldStateConfigurator
{
    public void Configure(IContainerBuilder builder) => Configure_Internal(builder);
    partial void Configure_Internal(IContainerBuilder builder);
}

public partial class SpellScriptsConfigurator
{
    public void Configure(IContainerBuilder builder) => Configure_Internal(builder);
    partial void Configure_Internal(IContainerBuilder builder);
}

public partial class AuraScriptsConfigurator
{
    public void Configure(IContainerBuilder builder) => Configure_Internal(builder);
    partial void Configure_Internal(IContainerBuilder builder);
}
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;

public abstract class Character : PoolableBehaviour, ISpellTarget, IAuraTarget
{
    [SerializeField] protected bool allowTick = true;
    [SerializeField] private TargetFlags targetFlags;
    
    public TargetFlags Flags => targetFlags;
    public Transform Transform => transform;
    public CancellationTokenSource DeadCTS { get; } = new();
    public CancellationToken CancellationToken => linkedCTS.Token;

    private AuraComponent auraComponent;
    private CancellationTokenSource linkedCTS;
    private HashSet<CharacterComponent> characterAbilities;
    
    private void Awake()
    {
        auraComponent = GetComponent<AuraComponent>();
        linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(DeadCTS.Token, destroyCancellationToken);
        characterAbilities = new HashSet<CharacterComponent> (GetComponents<CharacterComponent>());
        Create(); 
        characterAbilities.ForEach(ability => ability.Create());
    }

    // ReSharper disable once Unity.IncorrectMethodSignature
    private async UniTaskVoid Start()
    {
        var storedAllowTick = allowTick;
        allowTick = false;
        await Init(); characterAbilities.ForEach(ability => ability.Init());
        await PostInit();
        allowTick = storedAllowTick;
        
        
    }
    
    protected virtual void Create() { }
    
    protected virtual UniTask Init() { return UniTask.CompletedTask; }
    protected virtual UniTask PostInit() { return UniTask.CompletedTask; }

    private void Update()
    {
        if (!allowTick) return;
        
        Tick(Time.deltaTime);
#if !USE_ABILITY_UPDATE
        characterAbilities.ForEach(ability => ability.Tick(Time.deltaTime));
#endif
    }

    private void LateUpdate()
    {
        if (!allowTick) return;
        
        LateTick(Time.deltaTime);
#if !USE_ABILITY_UPDATE
        characterAbilities.ForEach(ability => ability.LateTick(Time.deltaTime));
#endif
    }
    
    protected virtual void Tick(float deltaTime)
    {
    }
    
    protected virtual void LateTick(float deltaTime)
    {
    }

    public virtual void Hit(Hit hit)
    {
    }

    protected virtual void OnDie() { }

    public void Die()
    {
        OnDie();
        DeadCTS.Cancel();
    }

    
    public override void Dispose()
    {
        characterAbilities.ForEach(ability => ability.Dispose());
    }

    public AuraComponent GetAuraComponent()
    {
        return auraComponent;
    }
}
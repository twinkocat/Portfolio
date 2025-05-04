using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Character : PoolableBehaviour, ISpellTarget
{
    [SerializeField] protected bool allowTick = true;
    [SerializeField] private TargetFlags targetFlags;
    
    public TargetFlags Flags => targetFlags;
    public Vector3 Position => transform.position;
    public CancellationTokenSource DestroyCTS { get; } = new CancellationTokenSource();
    
    private HashSet<CharacterAbility> characterAbilities;
    
    private void Awake()
    {
        characterAbilities = new HashSet<CharacterAbility> (GetComponents<CharacterAbility>());
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

    public override void Dispose()
    {
        DestroyCTS.Cancel();
        characterAbilities.ForEach(ability => ability.Dispose());
    }

    public virtual void Die() { }
}
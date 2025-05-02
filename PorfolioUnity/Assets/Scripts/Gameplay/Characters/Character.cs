using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;

public abstract class Character : PoolableBehaviour, IDamageable
{
    [SerializeField] protected bool allowTick = true;
    [SerializeField] private DamageableFlags damageableFlags;
    
    public DamageableFlags DamageableFlags => damageableFlags;
    public bool AllowTick => allowTick;

    private HashSet<CharacterAbility> characterAbilities;
    
    private void Awake()
    {
        characterAbilities = new HashSet<CharacterAbility> (GetComponents<CharacterAbility>());
        Init();
        characterAbilities.ForEach(ability => ability.Create());
        characterAbilities.ForEach(ability => ability.Init());
    }

    // ReSharper disable once Unity.IncorrectMethodSignature
    private async UniTaskVoid Start()
    {
        await PostInit();
        await UniTask.WaitUntil(() => allowTick);
    }
    
    protected virtual void Init() { }
    
    protected virtual UniTask PostInit() { return UniTask.CompletedTask; }

    private void Update()
    {
        if (!AllowTick) return;
        
        Tick(Time.deltaTime);
#if !USE_ABILITY_UPDATE
        characterAbilities.ForEach(ability => ability.Tick(Time.deltaTime));
#endif
    }

    private void LateUpdate()
    {
        if (!AllowTick) return;
        
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

    public virtual void Damage(Hit hit)
    {
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

[Serializable]
public abstract class SpellScript
{
    private UniTask castTask;
    private ISpellCaster owner;
    private SpellsAbility ownerAbility;
    
    public void Cast(Action successCallback, Action failCallback, Action castEndCallback)
    {
        if (!CanCast())
        {
            failCallback?.Invoke();
            return; 
        }
        Prepare();
        castTask = CastAsync(successCallback, castEndCallback);
    }

    private async UniTask CastAsync(Action successCallback, Action endCallback)
    {
        successCallback?.Invoke();
        
        await Activate(owner.CancellationToken);
        
        endCallback?.Invoke();
    }
    
    protected virtual bool CanCast() { return true; }
    protected virtual void Prepare() { }
    protected abstract UniTask Activate(CancellationToken cancellationToken);
    
    protected abstract void OnHit(ISpellTarget target);
    protected SpellsAbility GetOwnerAbility() { return ownerAbility; }
    
    protected ISpellCaster GetOwner() { return owner; }
    
    protected TOwner GetOwner<TOwner>(bool throwException = false) where TOwner : class
    {
        if (owner is TOwner tOwner)
            return tOwner;
        
        if (throwException)
            throw new NullReferenceException();
            
        return null;
    }
    
    public static SpellScript Create<T>(ISpellCaster spellCaster) where T : SpellScript, new()
    {
        var spellScript = Game.Resolver.Resolve<T>();
        spellScript.owner = spellCaster;
        spellScript.ownerAbility = spellCaster.GetSpellsAbility();
        return spellScript;
    }
}

public abstract class SpellScript<TData> : SpellScript where TData : SpellData
{
    [Inject] protected TData data;

    protected override bool CanCast()
    {
        if (GetOwner() == null)
        {
            Debug.LogWarning("No Owner is assigned to SpellScript");
            return false;
        }
        
        if (!data)
        {
            Debug.LogWarning($"No data found for {typeof(TData).Name}");
            return false;
        }
        
        if (HasCooldown() && GameTime.IsCooldown(data.id.ToString()))
            return false;
        
        GameTime.CommitCooldown(data.id.ToString(), GetCooldown(), OnCooldownTick, OnCooldownComplete, GetOwner().CancellationToken);
        return true;
    }
    
    protected virtual void OnCooldownTick(TimerData timerData) { }
    protected virtual void OnCooldownComplete() { }

    
    protected float GetCooldown()
    {
        var cooldownMod = data.tags.HasFlag(SpellTags.AffectByAttackSpeed)
            ? GetOwnerAbility().AttackSpeed.Value * data.attackSpeedMod
            : 0;
        
        return Mathf.Max(SpellData.MIN_COOLDOWN, data.baseCooldown - cooldownMod);
    }
    
    private bool HasCooldown() { return data.tags.HasFlag(SpellTags.HasCooldown); }
}

[Flags]
public enum SpellTags
{
    None = 0,
    HasCooldown = 1 << 0,
    AffectByAttackSpeed = 1 << 1,
}
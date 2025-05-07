using System;
using UnityEngine;

[RequireComponent(typeof(Character))]
public abstract class CharacterAbility : MonoBehaviour, IDisposable
{
    private Character owner;

    public void Create() => owner = gameObject.GetComponent<Character>();
    
    public virtual void Init()
    {
    }
    
#if USE_ABILITY_UPDATE
    protected virtual void Update()
    {
        Tick(Time.deltaTime);
    }    

    protected virtual void LateUpdate()
    {
        LateTick(Time.deltaTime);
    }
    
#endif

    public virtual void Tick(float deltaTime)
    {
    }

    public virtual void LateTick(float deltaTime)
    {
    }

    public Character GetOwner()
    {
        return owner;
    }
    
    public TOwner GetOwner<TOwner>(bool throwException = false) where TOwner : class
    {
        if (owner is TOwner tOwner)
            return tOwner;
        
        if (throwException)
            throw new NullReferenceException();
            
        return null;
    }

    public virtual void Dispose()
    {
    }
}
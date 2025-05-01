using System;
using UnityEngine;

[RequireComponent(typeof(Character))]
public abstract class CharacterAbility : MonoBehaviour
{
    private Character owner;

    public void Init()
    {
        owner = gameObject.GetComponent<Character>();
        Init(owner);
    }

    protected virtual void Init(Character character) { }
    
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
    
    public TOwner GetOwner<TOwner>(bool throwException = false) where TOwner : Character
    {
        var tOwner = owner as TOwner;
        
        return tOwner || !throwException ? tOwner : throw new NullReferenceException();
    }
}
using System;
using UnityEngine;

public interface IDamageable
{
    DamageableFlags DamageableFlags { get; }
    
    void Damage(Hit hit);
}

[Serializable]
public struct Hit
{
    public float rawDamage;
    public Vector3 hitPosition;
    
    public static Action<HitData> OnHitInvoke;
    
    public float InvokeDamage()
    {
        var damage = CalculateDamage();
        
        OnHitInvoke?.Invoke(new HitData
        {
            damage = damage,
            position = hitPosition,
        });

        return damage;
    }

    private float CalculateDamage()
    {
        return rawDamage;
    }
}

[Serializable]
public struct HitData
{
    public float damage;
    public Vector3 position;
}

[Flags]
public enum DamageableFlags
{
    None = 0,
    Player = 1 << 0,
    NPC = 1 << 1,
    Enemy = 1 << 2,
}
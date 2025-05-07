using System;
using UnityEngine;
using UnityEngine.Serialization;

public interface ISpellTarget
{
    TargetFlags Flags { get; }
    Transform Transform { get; }
    void Hit(Hit hit);
}

[Serializable]
public struct Hit
{
    public float rawHit;
    public HitType hitType;
    public Vector3 hitPosition;
    
    public static Action<HitData> OnHitInvoke;
    
    public float InvokeHit()
    {
        var hitPoints = CalculateHit();
        
        OnHitInvoke?.Invoke(new HitData
        {
            hitPoints = hitPoints,
            position = hitPosition,
            hitType = hitType,
        });

        return hitPoints;
    }

    private float CalculateHit()
    {
        return hitType switch
        {
            HitType.DirectDamage or HitType.DotDamage => -rawHit,
            HitType.DirectHeal or HitType.DotHeal => rawHit,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

[Serializable]
public struct HitData
{
    public float hitPoints;
    public HitType hitType;
    public Vector3 position;
}

public enum HitType
{
    None = 0,
    DirectDamage,
    DotDamage,
    DirectHeal,
    DotHeal,
}

[Flags]
public enum TargetFlags
{
    None = 0,
    Player = 1 << 0,
    NPC = 1 << 1,
    Enemy = 1 << 2,
}
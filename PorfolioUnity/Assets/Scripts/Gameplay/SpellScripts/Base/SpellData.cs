using System;
using MyBox;
using UnityEngine;

public abstract class SpellData : ScriptableObject
{
    [ReadOnly] public int id = -1;
    public Sprite icon;
    public SpellTags tags;
    public TargetFlags targetFlags;
    public float baseCooldown = 0;
    public float attackSpeedMod = 1F;
    
    public const float MIN_COOLDOWN = 0.1f;
    
    public bool debug = false;
    public DebugShapeData debugShapeData;
    
    private void OnValidate()
    {
        if (id == -1) id = Guid.NewGuid().GetHashCode();
    }
}


public abstract class ConeSpellData : SpellData
{
    public float lenght;
    public float angle;
}

public abstract class CircleSpellData : SpellData
{
    public float radius;
}
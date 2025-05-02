using System;
using UnityEngine;

public class HealthAbility : CharacterAbility
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;


    public override void Init()
    {
        health = maxHealth;
    }

    public void SetMaxHealth(float newMaxHealth, HealthSetMod mod = HealthSetMod.Default)
    {
        maxHealth = newMaxHealth;
        health = mod switch
        {
            HealthSetMod.Default => health,
            HealthSetMod.Adjust => health * (maxHealth / newMaxHealth),
            HealthSetMod.Regenerate => maxHealth,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public bool UpdateHealth(float deltaHealth)
    {
        health = Mathf.Clamp(health + deltaHealth, 0, maxHealth);
        return health != 0;
    }
}


public enum HealthSetMod
{
    Default,
    Adjust,
    Regenerate,
}

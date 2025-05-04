using System;
using UnityEngine;

public class HealthAbility : CharacterAbility
{
    [SerializeField] private ReactiveProperty<float> health;
    [SerializeField] private ReactiveProperty<float> maxHealth;

    public ReactiveProperty<float> Health => health;
    public ReactiveProperty<float> MaxHealth => maxHealth;

    public override void Init()
    {
        health.Value = maxHealth.Value;
    }

    public void SetMaxHealth(float newMaxHealth, HealthSetMod mod = HealthSetMod.Default)
    {
        maxHealth.Value = newMaxHealth;
        health.Value = mod switch
        {
            HealthSetMod.Default => health.Value,
            HealthSetMod.Adjust => health.Value * (maxHealth.Value / newMaxHealth),
            HealthSetMod.Regenerate => maxHealth.Value,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public bool UpdateHealth(float deltaHealth)
    {
        health.Value = Mathf.Clamp(health.Value + deltaHealth, 0, maxHealth.Value);
        return health.Value != 0;
    }

    public override void Dispose()
    {
        health.Dispose();
        maxHealth.Dispose();
    }
}


public enum HealthSetMod
{
    Default,
    Adjust,
    Regenerate,
}


public interface IHealthAbility
{
    HealthAbility GetHealthAbility();
}
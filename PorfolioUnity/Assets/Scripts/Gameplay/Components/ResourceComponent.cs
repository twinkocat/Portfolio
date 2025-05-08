using System;
using UnityEngine;

public class ResourceComponent : CharacterComponent
{
    [SerializeField] private ReactiveProperty<float> resource;
    [SerializeField] private ReactiveProperty<float> maxResource;
    
    public ReactiveProperty<float> Resource => resource;
    public ReactiveProperty<float> MaxResource => maxResource;

    public override void Init()
    {
    }

    public void SetMaxHealth(float newMaxHealth, HealthSetMod mod = HealthSetMod.Default)
    {
        maxResource.Value = newMaxHealth;
        resource.Value = mod switch
        {
            HealthSetMod.Default => resource.Value,
            HealthSetMod.Adjust => resource.Value * (maxResource.Value / newMaxHealth),
            HealthSetMod.Regenerate => maxResource.Value,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void UpdateMaxHealth(float deltaMaxHealth, HealthSetMod mod = HealthSetMod.Default)
    {
        var newMaxHealth = maxResource.Value + deltaMaxHealth;
        SetMaxHealth(newMaxHealth, mod);
    }

    public void UpdateResource(float deltaHealth)
    {
        resource.Value = Mathf.Clamp(resource.Value + deltaHealth, 0, maxResource.Value);
    }

    public override void Dispose()
    {
        resource.Dispose();
        maxResource.Dispose();
    }
}

public interface IResourceComponent
{
    ResourceComponent GetResourceComponent();
}
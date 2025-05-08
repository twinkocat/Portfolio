using UnityEngine;

public class TrainingDummy : Character
{
    private HealthComponent healthComponent;

    protected override void Create()
    {
        healthComponent = GetComponent<HealthComponent>();
    }

    public override void Hit(Hit hit)
    {
        var hitPoints = hit.InvokeHit();
        healthComponent.UpdateHealth(hitPoints);
    }
}

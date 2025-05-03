using UnityEngine;

public class TrainingDummy : Character
{
    private HealthAbility healthAbility;

    protected override void Create()
    {
        healthAbility = GetComponent<HealthAbility>();
    }

    public override void Hit(Hit hit)
    {
        var damage = hit.InvokeDamage();
        healthAbility.UpdateHealth(-damage);
    }
}

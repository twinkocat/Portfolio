public class TrainingDummy : Character
{
    private HealthAbility healthAbility;

    protected override void Init()
    {
        healthAbility = GetComponent<HealthAbility>();
    }

    public override void Damage(Hit hit)
    {
        var damage = hit.InvokeDamage();
    }
}

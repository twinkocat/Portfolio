
public class Warrior_Rage : AuraScript<Warrior_RageData>
{
    private int stacks;
    private SpellsComponent spellComponent;
    private HealthComponent healthComponent;
    
    public override void Apply()
    {
        spellComponent = GetVictim<ISpellCaster>(true).GetSpellComponent();
        healthComponent = GetVictim<IHealthComponent>(true).GetHealthComponent();
        spellComponent.OnCast += OnCast;
    }

    private void OnCast()
    {
        var maxStacks = stacks > data.maxStacks;

        if (maxStacks)
        {
            return;
        }
        ApplyStack();
        GameTime.CreateTimer(data.stackDuration, onComplete: RemoveStack);
    }

    private void ApplyStack()
    {
        spellComponent.AttackPower.Value += data.bonusDamage;
        healthComponent.UpdateMaxHealth(data.bonusHealth, HealthSetMod.Adjust);
        stacks++;
    }

    private void RemoveStack()
    {
        spellComponent.AttackPower.Value -= data.bonusDamage;
        healthComponent.UpdateMaxHealth(-data.bonusHealth, HealthSetMod.Adjust);
        stacks--;
    }
    
    public override void Dispose()
    {
        for (var i = 0; i < stacks; i++)
        {
            spellComponent.AttackPower.Value -= data.bonusDamage;
            healthComponent.UpdateMaxHealth(-data.bonusHealth, HealthSetMod.Adjust);
        }
        
        spellComponent.OnCast -= OnCast;
    }
}
public class HudMediator : Mediator<HudView>
{
    public override void Start()
    {
        //PlayerSpellScript.OnCooldownUpdated += OnCooldownUpdated;
        Game.OnPlayerSpawn += PlayerSpawn;

    }

    private void PlayerSpawn(Player player)
    {
        var ability = player.GetHealthComponent();

        View.hpBar.SetValues(ability.Health.Value, ability.MaxHealth.Value);
        ability.Health.PropertyChanged += View.hpBar.SetCurrentValue;
        ability.MaxHealth.PropertyChanged += View.hpBar.SetMaxValue;
    }
    

    private void OnCooldownUpdated(int index, float value)
    {
        View.spellView[index].SetTimerNormalized(value);
    }
    
    
    public SpellView GetSpellView(int index)
    {
        return View.spellView[index];
    }

    public override void Dispose()
    {
        //PlayerSpellScript.OnCooldownUpdated -= OnCooldownUpdated;

    }
}

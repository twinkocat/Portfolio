public class HudMediator : Mediator<HudView>
{
    public override void Start()
    {
        PlayerSpellScript.OnCooldownUpdated += OnCooldownUpdated;
        Game.OnPlayerSpawn += PlayerSpawn;

    }

    private void PlayerSpawn(Player player)
    {
        var ability = player.GetHealthAbility();

        View.hpBar.SetValues(ability.Health.Value, ability.MaxHealth.Value);
        ability.Health.PropertyChanged += View.hpBar.SetCurrentValue;
        ability.MaxHealth.PropertyChanged += View.hpBar.SetMaxValue;
    }
    

    private void OnCooldownUpdated(int index, float value)
    {
        View.spellView[index].SetTimerNormalized(value);
    }

    public StatusBar GetHealthStatusBar()
    {
        return View.hpBar;
    }
    
    public StatusBar GetEnergyStatusBar()
    {
        return View.resourceBar;
    }
    
    public SpellView GetSpellView(int index)
    {
        return View.spellView[index];
    }

    public override void Dispose()
    {
        PlayerSpellScript.OnCooldownUpdated -= OnCooldownUpdated;

    }
}

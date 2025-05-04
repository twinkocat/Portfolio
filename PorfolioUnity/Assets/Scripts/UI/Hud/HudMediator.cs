public class HudMediator : Mediator<HudView>
{
    public override void Start()
    {
        PlayerSpellScript.OnCooldownUpdated += OnCooldownUpdated;

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

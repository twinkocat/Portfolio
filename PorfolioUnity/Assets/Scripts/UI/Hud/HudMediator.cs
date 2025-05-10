using MyBox;

public class HudMediator : Mediator<HudView>
{
    public override void Start()
    {
        Player.OnCooldownUpdated += OnCooldownUpdated;
        Player.OnSpellSet += OnPlayerSpellSet;
        Game.OnPlayerSpawn += PlayerSpawn;
    }

    public void InitHud()
    {
        View.spellView.ForEach(spellView => spellView.SetTimerNormalized(0));
    }

    private void OnPlayerSpellSet(int index, SpellData spellData)
    {
        View.spellView[index].SetImage(spellData.icon);
    }

    private void PlayerSpawn(Player player)
    {
        var health = player.GetHealthComponent();
        var resource = player.GetResourceComponent();
        var spell = player.GetSpellComponent();
        var move = player.GetMovementComponent();
        
        
        View.hpBar.SetValues(health.Health.Value, health.MaxHealth.Value);
        health.Health.PropertyChanged += View.hpBar.SetCurrentValue;
        health.MaxHealth.PropertyChanged += View.hpBar.SetMaxValue;
        
        View.resourceBar.SetValues(resource.Resource.Value, resource.MaxResource.Value);
        resource.Resource.PropertyChanged += View.resourceBar.SetCurrentValue;
        resource.MaxResource.PropertyChanged += View.resourceBar.SetMaxValue;
     
        View.attackStat.InitStatValues("Attack Power", spell.AttackPower.Value);
        spell.AttackPower.PropertyChanged += View.attackStat.SetStatValue;
        
        View.attackSpeed.InitStatValues("Attack Speed", spell.AttackSpeed.Value);
        spell.AttackSpeed.PropertyChanged += View.attackSpeed.SetStatValue;
        
        View.moveSpeed.InitStatValues("Move Speed", move.CurrentSpeed.Value);
        move.CurrentSpeed.PropertyChanged += View.moveSpeed.SetStatValue;
    }
    

    private void OnCooldownUpdated(int index, float value)
    {
        View.spellView[index].SetTimerNormalized(value);
    }
    
    public override void Dispose()
    {
        Player.OnCooldownUpdated -= OnCooldownUpdated;
        Player.OnSpellSet -= OnPlayerSpellSet;
    }
}

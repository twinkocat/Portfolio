using System.Collections.Generic;
using VContainer;

public abstract class PlayerSpellScript : SpellScript
{
    public override bool CanExecute => !OnCooldownList.Contains(GetName());

    private SpellView spellView;
    private static readonly List<string> OnCooldownList = new();

    protected override void PreExecute()
    {
        spellView = Game.Resolver.Resolve<HudMediator>()
                                 .GetSpellView(GetSpellViewIndex());
    }

    protected void SetCooldown(float seconds)
    {
        GameTime.CreateCooldown(seconds, OnCooldown, () =>
        {
            OnCooldownList.Remove(GetName());
            OnCooldownEnd();
        }, spellView.destroyCancellationToken);
        OnCooldownList.Add(GetName());
    }

    protected virtual void OnCooldown(CooldownData data)
    {
        spellView.SetTimerNormalized(1 - data.GetNormalized());
    }

    protected virtual void OnCooldownEnd()
    {
        spellView.SetTimerNormalized(0);
    }
    
    protected virtual int GetSpellViewIndex() { return 0; }
}

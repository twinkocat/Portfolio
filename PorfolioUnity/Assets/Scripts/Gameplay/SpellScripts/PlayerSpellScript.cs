using System;
using System.Collections.Generic;

public abstract class PlayerSpellScript : SpellScript
{
    public static Action<int, float> OnCooldownUpdated;
    
    public override bool CanExecute => !OnCooldownList.Contains(GetName());
    protected abstract PlayerSpellType SpellViewIndex { get; }

    private static readonly List<string> OnCooldownList = new();

    
    protected void SetCooldown(float seconds)
    {
        GameTime.CreateTimer(seconds, OnCooldown, () =>
        {
            OnCooldownList.Remove(GetName());
            OnCooldownEnd();
        });
        
        OnCooldownList.Add(GetName());
    }

    protected virtual void OnCooldown(TimerData data)
    {
        OnCooldownUpdated?.Invoke(ToPlayerSpellInteger(SpellViewIndex), 1 - data.GetNormalized());
    }

    protected virtual void OnCooldownEnd()
    {
        OnCooldownUpdated?.Invoke(ToPlayerSpellInteger(SpellViewIndex), 0);

    }
    
    private static int ToPlayerSpellInteger(PlayerSpellType spellType)
    {
        return spellType switch
        {
            PlayerSpellType.Primary0 => 0,
            PlayerSpellType.Primary1 => 1,
            PlayerSpellType.Ultimate => 2,
            PlayerSpellType.Dash => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(spellType), spellType, null)
        };
    }
}

public enum PlayerSpellType
{
    Primary0 = 0,
    Primary1 = 1,
    Ultimate = 2,
    Dash = 3,
}

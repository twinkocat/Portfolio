using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpellsAbility : CharacterAbility
{
    [SerializeField] private ReactiveProperty<float> attackPower;
    [SerializeField] private ReactiveProperty<float> attackSpeed;
    [SerializeField] private ReactiveProperty<float> levelMod;
    [SerializeField] private Animator animator;

    public ReactiveProperty<float> AttackPower => attackPower;
    public ReactiveProperty<float> AttackSpeed => attackSpeed;
    public ReactiveProperty<float> LevelMod => levelMod;


    private readonly Dictionary<string, Func<SpellScript>> spellBindings = new();
    
    public void CastSpell(string spellName, Action successCallback = null, Action failureCallback = null, Action onCastEndCallback = null)
    {
        if (spellBindings.TryGetValue(spellName, out var spellFunc))
        {
            spellFunc().Cast(successCallback, failureCallback, onCastEndCallback);
        }        
    }

    public void BindAbility<T>(string spellName) where T : SpellScript, new()
    {
        spellBindings[spellName] = () => SpellScript.Create<T>(GetOwner<ISpellCaster>(true));
    }
    
    public void UnbindAbility(string spellName)
    {
        spellBindings.Remove(spellName);
    }

    public void UnbindAllAbilities()
    {
        spellBindings.Clear();
    }
}

public interface ISpellCaster
{
    ISpellTarget Victim { get; }
    CancellationToken CancellationToken { get; }
    Transform Transform { get; }
    SpellsAbility GetSpellsAbility();
    
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellsAbility : CharacterAbility
{
    [SerializeField] private Animator animator;
    
    private readonly Dictionary<string, Func<SpellScript>> spellBindings = new();
    
    public void CastSpell(string spellName)
    {
        if (spellBindings.TryGetValue(spellName , out var spellFunc))
        {
            spellFunc().Execute();
        }        
    }

    public void BindAbility<T>(string spellName) where T : SpellScript, new()
    {
        spellBindings[spellName] = () =>
        {
            var spell = new T();
            spell.Init(GetOwner(), animator);
            return spell;
        };
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
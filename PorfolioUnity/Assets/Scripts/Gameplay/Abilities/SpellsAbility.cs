using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellsAbility : CharacterAbility
{
    [SerializeField] private Animator animator;
    
    private readonly Dictionary<string, Func<SpellScript>> bindSpells = new();
    
    public override void Init()
    {
        
    }

    public void CastSpell(string spellName)
    {
        if (bindSpells.TryGetValue(spellName , out var spellFunc))
        {
            spellFunc().Execute();
        }        
    }

    public void BindAbility<T>(string spellName) where T : SpellScript, new()
    {
        bindSpells[spellName] = () =>
        {
            var spell = new T();
            spell.Init(GetOwner());
            return spell;
        };
    }


    public void UnbindAbility(string spellName)
    {
        bindSpells.Remove(spellName);
    }

    public void UnbindAllAbilities()
    {
        bindSpells.Clear();
    }
}
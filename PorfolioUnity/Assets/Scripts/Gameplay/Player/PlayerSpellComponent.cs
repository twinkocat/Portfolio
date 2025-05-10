using MyBox;

public class PlayerSpellComponent : SpellsComponent
{
    public void PlayerSet()
    {
        spellBindings.Values.ForEach(spell => spell.Invoke().OnPlayerSet());
    }
}

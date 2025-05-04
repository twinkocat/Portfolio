
using System;

public class HudMediator : Mediator<HudView>
{
    
    
    public SpellView GetSpellView(int index)
    {
        return View.spellView[index];
    }
}

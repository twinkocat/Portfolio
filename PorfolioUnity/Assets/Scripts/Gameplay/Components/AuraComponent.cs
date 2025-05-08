using System.Collections.Generic;

public class AuraComponent : CharacterComponent
{
    private readonly List<AuraScript> auras =  new List<AuraScript>();

    public void ApplyAura<T>() where T : AuraScript
    {
        var aura = AuraScript.Create<T>(GetOwner<IAuraTarget>());
        aura.NotifyDestroy += OnAuraDestroyed;
        aura.Apply();
        auras.Add(aura);
    }

    private void OnAuraDestroyed(AuraScript aura)
    {
        aura.NotifyDestroy -= OnAuraDestroyed;
        aura.Dispose();
        auras.Remove(aura);
    }

    public override void Dispose()
    {
        auras.ForEach(aura => aura.Dispose());
        auras.Clear();
    }
}

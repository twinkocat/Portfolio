using System;
using Cysharp.Threading.Tasks;
using VContainer;

[Serializable]
public abstract class AuraScript : IDisposable
{
    private IAuraTarget victim;
    private UniTask auraTask;

    public event Action<AuraScript> NotifyDestroy;
     
    public abstract void Apply();
    public abstract void Dispose();
    
    
    protected IAuraTarget GetVictim()
    {
        return victim;
    }

    protected T GetVictim<T>(bool throwException = false) where T : class
    {
        if (victim is T tVictim)
        {
            return tVictim;   
        }

        if (throwException)
        {
            throw new NullReferenceException();
        }
        
        return null;
    }
    
    public static AuraScript Create<T>(IAuraTarget auraVictim) where T : AuraScript
    {
        var aura = Game.Resolver.Resolve<T>();
        aura.victim = auraVictim;
        return aura;
    }

}

public abstract class AuraScript<TData> : AuraScript where TData : AuraData
{
    [Inject]
    protected TData data;
}


public interface IAuraTarget
{
    public AuraComponent GetAuraComponent();
}

using System;
using UnityEngine;

public abstract class PoolableBehaviour : MonoBehaviour, IPoolable, IDisposable
{
    public virtual void OnGet()
    {
    }

    public virtual void OnRelease()
    {
    }

    public virtual void Dispose()
    {
        
    }
    
    private void OnDestroy()
    {
        Dispose();
    }
}

public interface IPoolable
{
    void OnGet();
    void OnRelease();
}

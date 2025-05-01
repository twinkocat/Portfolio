using UnityEngine;

public abstract class PoolableBehaviour : MonoBehaviour, IPoolable
{
    public virtual void OnGet()
    {
    }

    public virtual void OnRelease()
    {
    }
}

public interface IPoolable
{
    void OnGet();
    void OnRelease();
}

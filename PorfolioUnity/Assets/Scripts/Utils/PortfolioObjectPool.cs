
using System;
using UnityEngine.Pool;

public class PortfolioObjectPool<T> : ObjectPool<T>  where T : class
{
    public PortfolioObjectPool(Func<T> createFunc, Action<T> actionOnGet = null, Action<T> actionOnRelease = null, Action<T> actionOnDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000) 
        : base(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize)
    {
        for (var i = 0; i < defaultCapacity; i++)
        {
            var item = createFunc();
            actionOnRelease?.Invoke(item);
        }
    }
}

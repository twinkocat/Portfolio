using System;
using UnityEngine;
using UnityEngine.Pool;

public class OverlapPool : ObjectPool<Collider[]>
{
    private static OverlapPool _instance;
    public static OverlapPool Instance => _instance ??= new OverlapPool(() => new Collider[9999], defaultCapacity: 10);
    
    private OverlapPool(Func<Collider[]> createFunc, Action<Collider[]> actionOnGet = null, Action<Collider[]> actionOnRelease = null, Action<Collider[]> actionOnDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000) 
        : base(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize) { }
}

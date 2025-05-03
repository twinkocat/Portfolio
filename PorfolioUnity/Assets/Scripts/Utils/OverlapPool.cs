using System;
using UnityEngine;

public class OverlapPool : ObjectPool<Collider[]>
{
    private static OverlapPool _instance;
    public static OverlapPool Instance => _instance ??= new OverlapPool(() => new Collider[9999], defaultCapacity: 50);
    
    private OverlapPool(Func<Collider[]> createFunc, Action<Collider[]> actionOnGet = null, Action<Collider[]> actionOnRelease = null, Action<Collider[]> actionOnDestroy = null, bool isExpandable = true, int defaultCapacity = 10) 
        : base(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, isExpandable, defaultCapacity) { }
}

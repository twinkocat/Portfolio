using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IObjectPool<T> where T : class
{
    private readonly bool isExpandable;

    private List<T> activeObjects;
    private Stack<T> objectStack;
    
    private readonly Func<T> createFunc;
    private readonly Action<T> actionOnGet;
    private readonly Action<T> actionOnRelease;
    private readonly Action<T> actionOnDestroy;

    /// <summary>
    ///     Creating object pool instance.
    /// </summary>
    public ObjectPool(Func<T> createFunc, Action<T> actionOnGet = null,  Action<T> actionOnRelease = null, Action<T> actionOnDestroy = null, bool isExpandable = true, int defaultCapacity = 10)
    {
        this.createFunc = createFunc;
        this.actionOnGet = actionOnGet;
        this.actionOnRelease = actionOnRelease;
        this.actionOnDestroy = actionOnDestroy;
        this.isExpandable = isExpandable;
        FillPool(defaultCapacity);
    }
    
    public int PoolCount { get; private set; }

    public T Peek()
    {
        return objectStack.TryPeek(out var obj) ? obj : null;
    }

    /// <summary>
    ///     Pop object from pool.
    /// </summary>
    public T Get(Transform parent = null)
    {
        if (objectStack.TryPop(out var obj))
        {
        }
        else if (isExpandable)
        {
            obj = CreateObject();
        }
        else
        {
            throw new PoolIsNotExpandableException();
        }
        
        actionOnGet?.Invoke(obj);
        activeObjects.Add(obj);
        return obj;
    }

    /// <summary>
    ///     Return object to pool.
    /// </summary>
    public void Release(T obj)
    {
        activeObjects.Remove(obj);
        objectStack.Push(obj);
        actionOnRelease?.Invoke(obj);
    }

    /// <summary>
    ///     Return all active objects to pool
    /// </summary>
    public void ReleaseAllObjects()
    {
        foreach (var obj in activeObjects)
            Release(obj);

        activeObjects.Clear();
    }

    /// <summary>
    ///     Destroy pool.
    /// </summary>
    public void DestroyPool()
    {
        foreach (var obj in activeObjects)
        {
            Release(obj);
        }

        foreach (var obj in objectStack)
        {
            actionOnDestroy?.Invoke(obj);
        }
        
        activeObjects.Clear();
        objectStack.Clear();
        PoolCount = 0;
    }

    /// <summary>
    ///     Fill object pool.
    /// </summary>
    private void FillPool(int count)
    {
        objectStack = new Stack<T>(count);
        activeObjects = new List<T>();

        for (var i = 0; i < count; i++)
        {
            objectStack.Push(CreateObject());
        }
    }

    /// <summary>
    ///     Create object
    /// </summary>
    private T CreateObject()
    {
        PoolCount++;
        return createFunc();
    }

    private class PoolIsNotExpandableException : Exception
    {
    }
}

public interface IObjectPool<T> where T : class
{
    int PoolCount { get; }
    T Peek();
    T Get(Transform parent = null);
    void Release(T obj);
    void ReleaseAllObjects();
    void DestroyPool();
}

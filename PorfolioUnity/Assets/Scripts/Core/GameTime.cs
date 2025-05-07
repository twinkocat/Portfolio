using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameTime : IDisposable
{
    private static readonly List<string> cooldownList = new();
    private static readonly CancellationTokenSource DisposeCts = new();
    
    public static void CreateTimer(float seconds, Action<TimerData> updateCallback = null, Action onComplete = null, Action onCancel = null, CancellationToken ct = default)
    {
        Timer(seconds, updateCallback, onComplete, onCancel, ct).Forget(); 
    }

    public static void CommitCooldown(string key, float cooldown, Action<TimerData> updateCallback = null, Action onComplete = null, CancellationToken ct = default)
    {
        Timer(cooldown, updateCallback, () => { cooldownList.Remove(key); onComplete?.Invoke(); }, () => cooldownList.Remove(key), ct).Forget(); 
        cooldownList.Add(key);
    }
    
    public static bool IsCooldown(string key) => cooldownList.Contains(key);
    
    private static async UniTaskVoid Timer(float seconds, Action<TimerData> updateCallback = null, Action onComplete = null, Action onCancel = null, CancellationToken ct = default)
    {
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(DisposeCts.Token, ct);
        
        for (var time = 0F; time < seconds; time += Time.deltaTime)
        {
            if (linkedCts.IsCancellationRequested)
            {
                onCancel?.Invoke();
                break;
            }
            
            await UniTask.NextFrame();

            var data = new TimerData
            {
                passedTime = time,
                timer = seconds,
            };
            
            updateCallback?.Invoke(data);
        }
        
        if (!linkedCts.IsCancellationRequested)
            onComplete?.Invoke();
    }
    
    public void Dispose()
    {
        DisposeCts.Cancel();
        DisposeCts.Dispose();
    }
}

[Serializable]
public struct TimerData
{
    public float passedTime;
    public float timer;
    
    public float GetNormalized() => Mathf.Clamp01(passedTime / timer);
    
    public float GetPercent() => GetNormalized() * 100f;
}

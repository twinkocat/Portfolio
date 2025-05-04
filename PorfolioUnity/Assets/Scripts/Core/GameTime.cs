using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameTime : IDisposable
{
    private static readonly CancellationTokenSource DisposeCts = new();
    
    public static void CreateCooldown(float seconds, Action<CooldownData> updateCallback = null, Action onComplete = null, CancellationToken ct = default)
    {
        Cooldown(seconds, updateCallback, onComplete, ct).Forget(); 
    }

    private static async UniTaskVoid Cooldown(float seconds, Action<CooldownData> updateCallback = null, Action onComplete = null, CancellationToken ct = default)
    {
        for (var time = 0F; time < seconds; time += Time.deltaTime)
        {
            if (ct.IsCancellationRequested)
            {
                break;
            }
            
            await UniTask.NextFrame();

            var data = new CooldownData
            {
                passedTime = time,
                cooldownTime = seconds,
            };
            
            updateCallback?.Invoke(data);
        }
        
        if (!ct.IsCancellationRequested)
            onComplete?.Invoke();
    }
    
    public void Dispose()
    {
        DisposeCts.Cancel();
        DisposeCts.Dispose();
    }
}

[Serializable]
public struct CooldownData
{
    public float passedTime;
    public float cooldownTime;
    
    public float GetNormalized() => Mathf.Clamp01(passedTime / cooldownTime);
    
    public float GetPercent() => GetNormalized() * 100f;
}

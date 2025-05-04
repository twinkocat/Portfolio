using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameTime : IDisposable
{
    private static readonly CancellationTokenSource DisposeCts = new();
    
    public static void CreateTimer(float seconds, Action<TimerData> updateCallback = null, Action onComplete = null, CancellationToken ct = default)
    {
        Timer(seconds, updateCallback, onComplete, ct).Forget(); 
    }

    private static async UniTaskVoid Timer(float seconds, Action<TimerData> updateCallback = null, Action onComplete = null, CancellationToken ct = default)
    {
        for (var time = 0F; time < seconds; time += Time.deltaTime)
        {
            if (ct.IsCancellationRequested)
            {
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
public struct TimerData
{
    public float passedTime;
    public float timer;
    
    public float GetNormalized() => Mathf.Clamp01(passedTime / timer);
    
    public float GetPercent() => GetNormalized() * 100f;
}

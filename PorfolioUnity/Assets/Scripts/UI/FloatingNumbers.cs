using UnityEngine;
using UnityEngine.Pool;

public class FloatingNumbers : MonoBehaviour
{
    [SerializeField] private Number numberPrefab;
    
    private IObjectPool<Number> pool;

    private void Awake()
    {
        pool = new PortfolioObjectPool<Number>(CreateNumber, OnGet, OnRelease, defaultCapacity: 5000);
        Hit.OnHitInvoke += OnHit;
    }

    private void OnHit(HitData data)
    {
        var number = pool.Get();
        
        number.SetCallback(ReleaseNumber);
        number.SetText(data.hitPoints);
        number.SetPosition(data.position);
    }

    private void OnDestroy()
    {
        Hit.OnHitInvoke += OnHit;
    }
    
    private Number CreateNumber() => Instantiate(numberPrefab, transform);
    private void ReleaseNumber(Number number) => pool.Release(number);
    private static void OnRelease(Number number) => number.gameObject.SetActive(false);
    private static void OnGet(Number number) => number.gameObject.SetActive(true);
}

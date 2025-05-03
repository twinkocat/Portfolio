using UnityEngine;
using UnityEngine.Pool;

public class DebugShape : MonoBehaviour
{
    [SerializeField] private RoundShape roundShape;

    private IObjectPool<RoundShape> pool;

    private static DebugShape Instance { get; set; }

    private void Awake()
    {
        Instance = this;
        pool = new PortfolioObjectPool<RoundShape>(CreateRoundShape, OnGet, OnRelease, defaultCapacity: 5000);
    }

    public static void CreateCone(Vector3 position, Vector3 rotation, float angle, float length, float lifetime)
    {
        var shape = Instance.pool.Get();
        shape.AdjustSettings(ReleaseCallback, position, rotation, angle, length, lifetime);
    }

    public static void CreateCircle(Vector3 position, float radius, float lifetime)
    {
        var shape = Instance.pool.Get();
        shape.AdjustSettings(ReleaseCallback, position, Vector3.zero, 360, radius, lifetime);
    }
    
    private static RoundShape CreateRoundShape() => Instantiate(Instance.roundShape, Instance.transform);
    private static void OnGet(RoundShape shape) => shape.gameObject.SetActive(true);
    private static void OnRelease(RoundShape shape) => shape.gameObject.SetActive(false);
    private static void ReleaseCallback(RoundShape shape) => Instance.pool.Release(shape);
}

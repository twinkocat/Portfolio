using UnityEngine;

public class DebugShape : MonoBehaviour
{
    [SerializeField] private RoundShape roundShape;

    private IObjectPool<RoundShape> pool;

    private static DebugShape Instance { get; set; }

    private void Awake()
    {
        Instance = this;
        pool = new ObjectPool<RoundShape>(CreateRoundShape, OnGet, OnRelease, defaultCapacity: 1000);
    }

    public static void CreateCone(Vector3 position, Vector3 rotation, float angle, float length, float lifetime, Color color)
    {
        var shape = Instance.pool.Get();
        shape.AdjustSettings(ReleaseCallback, position, rotation, angle, length, lifetime, color);
    }

    public static void CreateCircle(Vector3 position, float radius, float lifetime, Color color)
    {
        var shape = Instance.pool.Get();
        shape.AdjustSettings(ReleaseCallback, position, Vector3.zero, 360, radius, lifetime, color);
    }
    
    private static RoundShape CreateRoundShape()
    {
        var shape = Instantiate(Instance.roundShape, Instance.transform);
        shape.gameObject.SetActive(false);
        return shape;
    }

    private static void OnGet(RoundShape shape) => shape.gameObject.SetActive(true);
    private static void OnRelease(RoundShape shape) => shape.gameObject.SetActive(false);
    private static void ReleaseCallback(RoundShape shape) => Instance.pool.Release(shape);
}

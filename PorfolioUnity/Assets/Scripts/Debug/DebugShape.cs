using UnityEngine;

public class DebugShape : MonoBehaviour
{
    [SerializeField] private RoundShape roundShape;
    
    private static DebugShape _instance;
    
    public static  DebugShape Instance => _instance ??= FindFirstObjectByType<DebugShape>(FindObjectsInactive.Include);
    
    public static void CreateCone(Vector3 position, Vector3 rotation, float angle, float length, float lifetime)
    {
        var shape = Instantiate(Instance.roundShape, Instance.transform);
        shape.AdjustSettings(position, rotation, angle, length);

        if (lifetime > 0)
        {
            Destroy(shape.gameObject, lifetime);
        }
    }

    public static void CreateCircle(Vector3 position, float radius, float lifetime)
    {
        var shape = Instantiate(Instance.roundShape, Instance.transform);
        shape.AdjustSettings(position, Vector3.zero, 360, radius);
        
        if (lifetime > 0)
        {
            Destroy(shape.gameObject, lifetime);
        }
    }
}

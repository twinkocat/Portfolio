using UnityEngine;

public static class Helper
{
    public static readonly Vector3[] DirectionsVectors = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

    public static void DestroyAllChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            Object.Destroy(child.gameObject);
        }
    }
}

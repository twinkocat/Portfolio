using UnityEngine;

public static class MathHelpers
{
    public static float GaussianRandom()
    {
        return Mathf.Sqrt(-2.0f * Mathf.Log(1.0f - Random.value)) * Mathf.Sin(2.0f * Mathf.PI * 1.0f - Random.value);
    }

    public static float GaussianRandom(float min, float max)
    {
        return min + max * GaussianRandom();
    }
    
    public static Vector2 GaussianPointInCircle(float min, float max)
    {
        var radius = Mathf.Abs(GaussianRandom(min, max));
        var angle = Random.Range(0f, Mathf.PI * 2);
        var x = Mathf.Cos(angle) * radius;
        var y = Mathf.Sin(angle) * radius;

        return new Vector2(x, y);
    }
    
    public static Vector3 GetX0Z(this Vector2 target, float y = 0F) => new Vector3(target.x, y, target.y); 
}
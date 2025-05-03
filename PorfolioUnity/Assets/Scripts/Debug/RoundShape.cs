using System;
using UnityEngine;

public class RoundShape : MonoBehaviour
{
    private static readonly int AngleDeg = Shader.PropertyToID("_AngleDeg");
    
    [SerializeField] private MeshRenderer meshRenderer;

    private Action<RoundShape> callback;
    
    public void AdjustSettings(Action<RoundShape> releaseCallback, Vector3 position, Vector3 rotation, float angle, float length, float lifetime)
    {
        callback = releaseCallback;
        meshRenderer.material.SetFloat(AngleDeg, angle);
        transform.position = new Vector3(position.x, position.y + 0.005F, position.z);
        transform.rotation = rotation.sqrMagnitude > 0 ? Quaternion.LookRotation(rotation, Vector3.up) : Quaternion.identity;
        transform.localScale = new Vector3(length, length, length);
        
        Invoke(nameof(Release), lifetime);
        
    }

    private void Release()
    {
        callback?.Invoke(this);
        callback = null;
    }
}

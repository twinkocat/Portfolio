using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Number : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private Action<Number> releaseCallback;
    
    public void SetCallback(Action<Number> callback)
    {
        releaseCallback = callback;
    }
    
    public void SetText(float number)
    {
        text.text = number.ToString(CultureInfo.InvariantCulture);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetColor(Color color)
    {
        text.color = color;
    }

    public void ReleaseNumber()
    {
        releaseCallback?.Invoke(this);
        releaseCallback = null;
    }
}

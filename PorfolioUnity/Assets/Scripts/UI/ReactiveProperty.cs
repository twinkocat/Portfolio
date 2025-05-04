using System;
using UnityEngine;

[Serializable]
public class ReactiveProperty<T> : IDisposable where T : IComparable, IComparable<T>, IEquatable<T>
{
    [SerializeField] private T m_value;

    public ReactiveProperty() => m_value = default;
    public ReactiveProperty(T value) => m_value = value;

    public T Value
    {
        get => m_value;
        set
        {
            if (m_value.Equals(value))
            {
                return;
            }

            m_value = value;
            PropertyChanged?.Invoke(value);
        }
    }
    
    public event Action<T> PropertyChanged;

    public void Dispose()
    {
        PropertyChanged = null;
    }
}

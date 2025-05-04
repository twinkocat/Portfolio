using UnityEngine;

public abstract class View : MonoBehaviour
{
    [SerializeField] private int screenOrder = 0;
    
    public int ScreenOrder => screenOrder;
}

using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    public UnityEvent<Collider> onTriggerEnterAction;
    public UnityEvent<Collider> onTriggerExitAction;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerEnterAction?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerExitAction?.Invoke(other);
        }
    }
}

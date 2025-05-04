using UnityEngine;
using UnityEngine.UI;

public class SpellView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image cooldown;
    
    public void SetImage(Sprite sprite)
    {
        
    }

    public void SetTimerNormalized(float normalizedTime)
    {
        if (cooldown)
        {
            cooldown.fillAmount = normalizedTime;
        }
    }

    public void OnComplete()
    {
        if (cooldown)
        {
            cooldown.fillAmount = 0;
        }
    }
}

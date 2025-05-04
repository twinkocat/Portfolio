using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text text;

    
    public void SetValues(float currentValue, float maxValue)
    {
        SetMaxValue(maxValue);
        SetCurrentValue(currentValue);
        UpdateText();
    }

    public void SetMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
    }
    
    public void SetCurrentValue(float currentValue)
    {
        slider.value = currentValue;
    }

    private void UpdateText()
    {
        text.text = $"{slider.maxValue}/{slider.maxValue}";
    }
}

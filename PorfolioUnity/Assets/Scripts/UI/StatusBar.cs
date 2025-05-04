using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;

    
    public void SetValues(float currentValue, float maxValue)
    {
        SetMaxValue(maxValue);
        SetCurrentValue(currentValue);
        UpdateText();
    }

    public void SetMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
        UpdateText();
    }
    
    public void SetCurrentValue(float currentValue)
    {
        slider.value = currentValue;
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = $"{slider.value}/{slider.maxValue}";
    }
}

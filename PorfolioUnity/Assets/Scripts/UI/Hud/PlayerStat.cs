using System.Globalization;
using TMPro;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private TMP_Text statName;
    [SerializeField] private TMP_Text statValue;


    public void InitStatValues(string newName, float newValue)
    {
        statName.text = newName;
        statValue.text = newValue.ToString(CultureInfo.InvariantCulture);
    }

    public void SetStatValue(float newValue)
    {
        statValue.text = newValue.ToString(CultureInfo.InvariantCulture);
    }
}

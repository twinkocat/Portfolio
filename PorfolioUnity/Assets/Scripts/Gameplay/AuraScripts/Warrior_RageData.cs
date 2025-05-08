using UnityEngine;

[CreateAssetMenu(menuName = "AuraData/Warrior_RageData")]
public class Warrior_RageData : AuraData
{
    public int maxStacks = 5;
    public float stackDuration = 2f;
    public float bonusHealth;
    public float bonusDamage;
}
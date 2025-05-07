using UnityEngine;

[CreateAssetMenu(menuName = "SpellData/SkeletonWarrior_SlashData", fileName = "SkeletonWarrior_SlashData")]
public class SkeletonWarrior_SlashData : ConeSpellData
{
    public float validDistance = 0.25F;
    public float castTime;
    public float posCastTime = 0.25F;
    public float damage;
}

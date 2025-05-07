using UnityEngine;

[CreateAssetMenu(menuName = "SpellData/Skeleton_SacrificeData", fileName = "Skeleton_SacrificeData")]
public class Skeleton_SacrificeData : CircleSpellData
{
    public Range maxHealTargets = new(1, 4);
    public float castTime = 1F;
    public float heal;
    public float damage;
}
using UnityEngine;

[CreateAssetMenu(menuName = "SpellData/SkeletonWarrior_ChargeData", fileName = "SkeletonWarrior_ChargeData")]
public class SkeletonWarrior_ChargeData : ChargeSpellData
{
    public float castTime = 0.5F;
    public Range chargeRange = new (2F, 5F);
}

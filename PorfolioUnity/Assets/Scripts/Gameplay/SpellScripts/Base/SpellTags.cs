using System;

[Flags]
public enum SpellTags
{
    None = 0,
    HasCooldown = 1 << 0,
    AffectByAttackSpeed = 1 << 1,
    PlayerSpell = 1 << 2,
}
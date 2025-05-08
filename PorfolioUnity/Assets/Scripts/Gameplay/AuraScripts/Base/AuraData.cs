using System;
using UnityEngine;

public abstract class AuraData : ScriptableObject
{
    public AuraTags auraTags;
}

[Flags]
public enum AuraTags
{
    None = 0,
    HasDuration = 1 << 0,
    HasStacks = 1 << 1,
}
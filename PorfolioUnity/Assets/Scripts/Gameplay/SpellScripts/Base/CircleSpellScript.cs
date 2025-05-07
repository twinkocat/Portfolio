using UnityEngine;

public abstract class CircleSpellScript<TData> : SpellScript<TData> where TData : CircleSpellData
{
    protected void CircleSpell_Internal(Vector3 position)
    {
        var circleResult = OverlapPool.Instance.Get();
        var count = Physics.OverlapSphereNonAlloc(position, data.radius, circleResult);

        if (data.debug)
        {
            DebugShape.CreateCircle(position, data.radius, data.debugShapeData.lifetime, data.debugShapeData.color);
        }
        
        for (var i = 0; i < count; i++)
        {
            if (circleResult[i].transform.TryGetComponent(out ISpellTarget spellTarget)
                && data.targetFlags.HasFlag(spellTarget.Flags))
            {
                OnHit(spellTarget);
            }
        }
        OverlapPool.Instance.Release(circleResult);
    }
}
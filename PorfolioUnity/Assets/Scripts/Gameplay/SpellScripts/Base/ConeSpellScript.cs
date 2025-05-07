using UnityEngine;

public abstract class ConeSpellScript<TData> : SpellScript<TData> where TData : ConeSpellData
{
    protected void ConeSpell_Internal(Vector3 position, Vector3 direction)
    {
        var coneResult = OverlapPool.Instance.Get();
        var count = ConeOverlapNonAlloc(position, direction, data.lenght, data.angle, coneResult);

        if (data.debug)
        {
            DebugShape.CreateCone(position, direction, data.angle, data.lenght, data.debugShapeData.lifetime, data.debugShapeData.color);
        }
        
        for (var i = 0; i < count; i++)
        {
            if (coneResult[i].transform.TryGetComponent(out ISpellTarget spellTarget) 
                && spellTarget.Flags.HasFlag(data.targetFlags))
            {
                OnHit(spellTarget);
            }
        }
    
        OverlapPool.Instance.Release(coneResult);
    }
    
    private static int ConeOverlapNonAlloc(Vector3 position, Vector3 forward, float length, float angle, Collider[] results)
    {
        var overlapResult = OverlapPool.Instance.Get();
        var count = Physics.OverlapSphereNonAlloc(position, length, overlapResult);

        var j = 0;
    
        for (var i = 0; i < count; i++)
        {
            var targetTransform = overlapResult[i].transform;
            var direction = (targetTransform.position - position).normalized;
            var angleToTarget = Vector3.Angle(forward, direction);
        
            if (angleToTarget > angle / 2f) continue;
        
            results[j] = overlapResult[i];
            j++;
        }
    
        OverlapPool.Instance.Release(overlapResult);

        return j;
    }
}
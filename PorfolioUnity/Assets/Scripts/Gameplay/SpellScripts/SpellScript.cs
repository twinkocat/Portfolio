using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public abstract class SpellScript
{
    public virtual bool CanExecute => true;
    public bool Done => executeTask.Status is not UniTaskStatus.Pending;
    
    private CancellationTokenSource tokenSource; 
    private UniTask executeTask;
    private Character owner;
    private Animator animator;
    private string name;

    public void Init(Character spellOwner, Animator ownerAnimator, string spellName)
    {
        name = spellName;
        owner = spellOwner;
        animator = ownerAnimator;
    }

    public void Execute()
    {
        if (!CanExecute)
        {
            return;
        }
        PreExecute();
        tokenSource = CancellationTokenSource.CreateLinkedTokenSource(owner.DeadCTS.Token, owner.destroyCancellationToken);
        executeTask = UniTask.Create(ExecuteSpellAsync, tokenSource.Token);
    }

    public void Cancel()
    {
        tokenSource.Cancel();
    }
    
    protected virtual void PreExecute() { }
    protected abstract UniTask ExecuteSpellAsync(CancellationToken cancellationToken);
    
    protected virtual void OnHit(ISpellTarget target) { }
    
    protected Animator GetAnimator() { return animator; }
    
    protected Character GetOwner() { return owner; }
    
    protected string GetName() { return name; }
    
    protected TOwner GetOwner<TOwner>(bool throwException = false) where TOwner : Character
    {
        var tOwner = owner as TOwner;
        
        return tOwner || !throwException ? tOwner : throw new NullReferenceException();
    }
    
    protected async UniTask ExecuteConeSpell(Vector3 position, Vector3 direction, float lenght, float angle, TargetFlags flags)
    {
        await ExecuteConeSpell_Internal(position, direction, lenght, angle, flags, OnHit);
    }

    protected async UniTask ExecuteCircleSpell(Vector3 position, float radius, TargetFlags flags)
    {
        await ExecuteCircleSpell_Internal(position, radius, flags, OnHit);
    }
    
    private static async UniTask ExecuteConeSpell_Internal(Vector3 position, Vector3 direction, float lenght, float angle, TargetFlags flags, Action<ISpellTarget> onHitCallback = null)
    {
        var coneResult = OverlapPool.Instance.Get();
        var count = ConeOverlapNonAlloc(position, direction, angle, lenght, coneResult);
        
        for (var i = 0; i < count; i++)
        {
            if (coneResult[i].transform.TryGetComponent(out ISpellTarget spellTarget) && spellTarget.Flags.HasFlag(flags))
            {
                onHitCallback?.Invoke(spellTarget);
            }
            await UniTask.Yield();
        }
        
        OverlapPool.Instance.Release(coneResult);
    }

    private static async UniTask ExecuteCircleSpell_Internal(Vector3 position, float radius, TargetFlags flags, Action<ISpellTarget> onHitCallback = null)
    {
        var circleResult = OverlapPool.Instance.Get();
        var count = Physics.OverlapSphereNonAlloc(position, radius, circleResult);

        for (var i = 0; i < count; i++)
        {
            if (   circleResult[i].transform.TryGetComponent(out ISpellTarget spellTarget)
                   && spellTarget.Flags.HasFlag(flags))
            {
                onHitCallback?.Invoke(spellTarget);
            }
            await UniTask.Yield();
        }
        OverlapPool.Instance.Release(circleResult);
    }
    
    private static int ConeOverlapNonAlloc(Vector3 position, Vector3 forward, float angle, float length, Collider[] results)
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

    public static T Create<T>(Character spellOwner, Animator ownerAnimator, string spellName) where T : SpellScript, new()
    {
        var spell = new T();
        spell.Init(spellOwner, ownerAnimator, spellName);
        return spell;
    }
}
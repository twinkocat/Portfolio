using System;
using UnityEngine;

public class RandomAnimationBehaviour : StateMachineBehaviour
{
    [SerializeField] private string[] stateNames = Array.Empty<string>();
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var index = UnityEngine.Random.Range(0, stateNames.Length);
        var stateName = stateNames[index];

        animator.Play(stateName, layerIndex);
    }
}
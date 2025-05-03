
using ZLinq;
using UnityEngine;

// Auto-generated animation event handler methods
public class NumberAnimationEventHandler : MonoBehaviour
{
    private AnimationEventHandler animationEventHandler;

    private void Awake() 
    {
        animationEventHandler = GetComponent<AnimationEventHandler>();
    }


    public void Release(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0f)
        {
            var wrapperEvent = animationEventHandler.animationEvents.AsValueEnumerable().FirstOrDefault(x => x.eventName == "Release");
            if (wrapperEvent == null) return;
            wrapperEvent.@event.Invoke();
        }
    }

}

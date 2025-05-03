using UnityEngine;
using VContainer;

public class GameEntryPoint : MonoBehaviour
{
    [Inject]
    private Game game;
    
    private void Start()
    {
        Game.SetState<InitializationState>();

        Hit.OnHitInvoke += OnHit;
    }

    private void OnHit(HitData hitData)
    {
        Debug.Log($"Hit: {hitData.hitPoints}");
    }
}

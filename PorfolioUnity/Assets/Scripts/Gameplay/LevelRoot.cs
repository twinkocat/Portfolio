using UnityEngine;
using VContainer;

public class LevelRoot : MonoBehaviour
{
    private Player playerInstance;

    private void Start()
    {
        playerInstance = Game.Resolver.Resolve<Player>();
    }

    public void BackToHub()
    {
        Game.SetState<HubState>();
    }
}

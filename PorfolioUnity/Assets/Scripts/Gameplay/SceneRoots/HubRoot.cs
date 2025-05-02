using UnityEngine;
using VContainer;

public class HubRoot : MonoBehaviour
{
    private Player playerInstance;

    private void Start()
    {
        playerInstance = Game.Resolver.Resolve<Player>();
    }

    public void StartGame()
    {
        Game.SetState<LevelState>();
    }
}

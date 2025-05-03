using UnityEngine;
using VContainer;

public class GameEntryPoint : MonoBehaviour
{
    [Inject]
    private Game game;
    
    [Inject]
    private GameTime gameTime;
    
    private void Start()
    {
        Game.SetState<InitializationState>();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class GameEntryPoint : MonoBehaviour
{
    [Inject]
    private Game game;
    
    private void Start()
    {
        Game.SetState<InitializationState>();
    }
}

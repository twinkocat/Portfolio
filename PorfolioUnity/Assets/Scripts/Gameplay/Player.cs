using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference dashAction;

    private DashAbility dashAbility;

    protected override void Init()
    {
        dashAbility = GetComponent<DashAbility>();
    }

    protected override UniTask PostInit()
    {
        dashAction.action.started += Dash;
        return base.PostInit();
    }
    
    private void Dash(InputAction.CallbackContext _)
    {
        dashAbility.Dash();
    }

    protected override void Tick(float deltaTime)
    {
        var input2D = moveAction.action.ReadValue<Vector2>();
        var input3D = new Vector3(input2D.x, 0f, input2D.y);
        CharacterMovement.SetDestination(transform.position + input3D);
    }

    private void OnDestroy()
    {
        dashAction.action.started -= Dash;
    }
}
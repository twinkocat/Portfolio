using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference dashAction;

    private MovementAbility movementAbility;
    private DashAbility dashAbility;
    private HealthAbility healthAbility;

    protected override void Init()
    {
        dashAbility = GetComponent<DashAbility>();
        movementAbility = GetComponent<MovementAbility>();
        healthAbility = GetComponent<HealthAbility>();
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
        var input3D = Game.IsometricMod * new Vector3(input2D.x, 0f, input2D.y);
        movementAbility.SetDestination(transform.position + input3D);
    }

    public override void Damage(Hit hit)
    {
        var damage = hit.InvokeDamage();
        
        if (!healthAbility.UpdateHealth(damage))
        {
            Debug.Log("Dead");    
        }
    }

    public override void Dispose()
    {
        dashAction.action.started -= Dash;
    }
}
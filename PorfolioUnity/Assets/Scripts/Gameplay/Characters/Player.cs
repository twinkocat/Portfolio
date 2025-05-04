using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private const string WARRIOR_CHARGE = "WARRIOR_CHARGE";
    private const string WARRIOR_SLASH = "WARRIOR_SLASH";
    private const string WARRIOR_SMASH = "WARRIOR_SMASH";
    private const string WARRIOR_EARTH_SHATTER = "WARRIOR_EARTH_SHATTER";
    
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference dashAction;
    [SerializeField] private InputActionReference firstAbilityAction;
    [SerializeField] private InputActionReference secondAbilityAction;
    [SerializeField] private InputActionReference ultimateAbilityAction;
    
    
    private MovementAbility movementAbility;
    private HealthAbility healthAbility;
    private SpellsAbility spellsAbility;
    
    protected override void Create()
    {
        movementAbility = GetComponent<MovementAbility>();
        healthAbility = GetComponent<HealthAbility>();
        spellsAbility = GetComponent<SpellsAbility>();
    }

    protected override UniTask Init()
    {
        dashAction.action.started += Dash;
        firstAbilityAction.action.started += FirstAbility;
        secondAbilityAction.action.started += SecondAbility;
        ultimateAbilityAction.action.started += UltimateAbility;
        
        spellsAbility.BindAbility<Warrior_ChargeSpellScript>(WARRIOR_CHARGE);
        spellsAbility.BindAbility<Warrior_SlashSpellScript>(WARRIOR_SLASH);
        spellsAbility.BindAbility<Warrior_SmashSpellScript>(WARRIOR_SMASH);
        spellsAbility.BindAbility<Warrior_EarthShatterSpellScript>(WARRIOR_EARTH_SHATTER);
        return base.Init();
    }
    
    private void Dash(InputAction.CallbackContext _)
    {
        spellsAbility.CastSpell(WARRIOR_CHARGE);
    }
    
    private void FirstAbility(InputAction.CallbackContext _)
    {
        spellsAbility.CastSpell(WARRIOR_SLASH);
    }
    
    private void SecondAbility(InputAction.CallbackContext _)
    {
        spellsAbility.CastSpell(WARRIOR_SMASH);

    }
    
    private void UltimateAbility(InputAction.CallbackContext _)
    {
        spellsAbility.CastSpell(WARRIOR_EARTH_SHATTER);

    }

    protected override void Tick(float deltaTime)
    {
        var input2D = moveAction.action.ReadValue<Vector2>();
        var input3D = Game.IsometricMod * new Vector3(input2D.x, 0f, input2D.y);
        movementAbility.SetDestination(transform.position + input3D);
    }

    public override void Hit(Hit hit)
    {
        var hitPoints = hit.InvokeHit();
        if (!healthAbility.UpdateHealth(hitPoints))
        {
            Debug.Log("Dead");    
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        dashAction.action.started -= Dash;
        firstAbilityAction.action.started -= FirstAbility;
        secondAbilityAction.action.started -= SecondAbility;
        ultimateAbilityAction.action.started -= UltimateAbility;
    }
}
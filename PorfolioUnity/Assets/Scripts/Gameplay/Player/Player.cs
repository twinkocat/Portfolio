using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class Player : Character, IHealthComponent, ISpellCaster, IMovementComponent, IResourceComponent
{
    public static Action<int, float> OnCooldownUpdated;
    public static Action<int, SpellData> OnSpellSet;
    
    private const string WARRIOR_CHARGE = "WARRIOR_CHARGE";
    private const string WARRIOR_SLASH = "WARRIOR_SLASH";
    private const string WARRIOR_SMASH = "WARRIOR_SMASH";
    private const string WARRIOR_EARTH_SHATTER = "WARRIOR_EARTH_SHATTER";
    
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference dashAction;
    [SerializeField] private InputActionReference firstAbilityAction;
    [SerializeField] private InputActionReference secondAbilityAction;
    [SerializeField] private InputActionReference ultimateAbilityAction;
    
    [Inject] private HudMediator hudMediator;
    
    public ISpellTarget Victim => null;
    
    private MovementComponent movementComponent;
    private HealthComponent healthComponent;
    private ResourceComponent resourceComponent;
    private PlayerSpellComponent spellsComponent;
    
    protected override void Create()
    {
        movementComponent = GetComponent<MovementComponent>();
        healthComponent = GetComponent<HealthComponent>();
        spellsComponent = GetComponent<PlayerSpellComponent>();
        resourceComponent = GetComponent<ResourceComponent>();
    }

    protected override async UniTask Init()
    {
        hudMediator.InitHud();
        dashAction.action.started += Dash;
        firstAbilityAction.action.started += FirstAbility;
        secondAbilityAction.action.started += SecondAbility;
        ultimateAbilityAction.action.started += UltimateAbility;
        
        GetAuraComponent().ApplyAura<Warrior_Rage>();
        spellsComponent.BindAbility<Warrior_Charge>(WARRIOR_CHARGE);
        spellsComponent.BindAbility<Warrior_Slash>(WARRIOR_SLASH);
        spellsComponent.BindAbility<Warrior_Smash>(WARRIOR_SMASH);
        spellsComponent.BindAbility<Warrior_EarthShatter>(WARRIOR_EARTH_SHATTER);
        spellsComponent.PlayerSet();
        
        await UniTask.DelayFrame(1);
        
        hudMediator.Show();
    }

    private void Dash(InputAction.CallbackContext _)
    {
        spellsComponent.CastSpell(WARRIOR_CHARGE);
    }
    
    private void FirstAbility(InputAction.CallbackContext _)
    {
        spellsComponent.CastSpell(WARRIOR_SLASH);
    }
    
    private void SecondAbility(InputAction.CallbackContext _)
    {
        spellsComponent.CastSpell(WARRIOR_SMASH);

    }
    
    private void UltimateAbility(InputAction.CallbackContext _)
    {
        spellsComponent.CastSpell(WARRIOR_EARTH_SHATTER);

    }

    protected override void Tick(float deltaTime)
    {
        var input2D = moveAction.action.ReadValue<Vector2>();
        var input3D = Game.IsometricMod * new Vector3(input2D.x, 0f, input2D.y);
        movementComponent.SetDestination(transform.position + input3D);
    }

    public override void Hit(Hit hit)
    {
        var hitPoints = hit.InvokeHit();
        if (!healthComponent.UpdateHealth(hitPoints))
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

    public HealthComponent GetHealthComponent()
    {
        return healthComponent;
    }
    
    public SpellsComponent GetSpellComponent()
    {
        return spellsComponent;
    }

    public MovementComponent GetMovementComponent()
    {
        return movementComponent;
    }

    public ResourceComponent GetResourceComponent()
    {
        return resourceComponent;
    }
}
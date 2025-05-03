using Cysharp.Threading.Tasks;

public class Enemy_Skeleton : Character
{
    private const string SKELETON_SACRIFICE = "SKELETON_SACRIFICE";
    
    private Player victim;
    
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
        spellsAbility.BindAbility<Skeleton_Sacrifice>(SKELETON_SACRIFICE);
        return base.Init();
    }

    public override void OnGet()
    {
    }

    public override void OnRelease()
    {
    }

    protected override void Tick(float deltaTime)
    {
        if (!victim)
        {
            return;
        }
        
        movementAbility.SetDestination(victim.transform.position);
    }

    public override void Hit(Hit hit)
    {
        hit.InvokeHit();
        healthAbility.UpdateHealth(-9999f);
        spellsAbility.CastSpell(SKELETON_SACRIFICE);
    }
}

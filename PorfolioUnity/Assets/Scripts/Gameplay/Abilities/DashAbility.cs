using UnityEngine;

public class DashAbility : CharacterAbility
{
    private static readonly int DashTrigger = Animator.StringToHash("Dash");
    
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationCurve dashCurve;
    [SerializeField] private float dashDuration = 0.25F;
    [SerializeField] private float dashSpeed = 5F;
    [SerializeField] private float cooldownDuration = 0.25F;
    
    private float lastDashTime;
    private float dashTimeCounter;
    private bool dashing;
    
    public void Dash()
    {
        if (Time.time - lastDashTime < cooldownDuration)
            return;
        
        animator.SetTrigger(DashTrigger);
        dashing = true;
        dashTimeCounter = 0;
        lastDashTime = Time.time;
    }

    public void AdjustCooldown(float cooldown)
    {
        cooldownDuration = cooldown;
    }
    
    public override void Tick(float deltaTime)
    {
        if (!dashing) 
            return;
        
        dashTimeCounter += deltaTime;
        dashing = dashTimeCounter < dashDuration;
        transform.position += transform.forward * (dashSpeed * dashCurve.Evaluate(Mathf.Clamp01(dashTimeCounter / dashDuration)) * deltaTime);
    }
}

using MyBox;
using UnityEngine;

public class MovementComponent : CharacterComponent
{
    private static readonly int MoveAnimSpeed = Animator.StringToHash("MoveAnimSpeed");
    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    
    [SerializeField] private Animator animator;
    
    [Separator("Movement")]
    [SerializeField] private float minMoveSpeed = 0.1F;
    [SerializeField] private float defaultMoveSpeed = 2F;
    [SerializeField] private float maxMoveSpeed = 5F;
    [SerializeField] private float rotationSpeed = 10F;
    
    [Separator("Animation")]
    [SerializeField] private float defaultMoveAnimSpeed = 1.0f;
    [SerializeField] private float maxMoveAnimSpeed = 2.0F;

    public ReactiveProperty<float> CurrentSpeed { get; private set; } = new();
    

    private float moveSpeed;
    private float moveMagnitude;
    private float animSpeedNormalized;
    private Vector3 moveVector;
    private Quaternion lookRotation;

    public override void Init()
    {
        CurrentSpeed.PreChangeCommit = (value) => Mathf.Clamp(value, minMoveSpeed, maxMoveSpeed);
        CurrentSpeed.Value = defaultMoveSpeed;
    }

    public void SetDestination(Vector3 destination)
    {
        moveVector = (destination -  transform.position).normalized;
        lookRotation = moveVector.sqrMagnitude > 0 ? Quaternion.LookRotation(moveVector) : transform.rotation;
    }
    
    public void SetMoveVector(Vector3 vector)
    {
        moveVector = vector;
    }

    public void StopMovement()
    {
        moveVector = Vector3.zero;
    }
    
    public override void Tick(float deltaTime)
    {
        moveMagnitude = moveVector.magnitude;
        moveSpeed = moveVector.magnitude * CurrentSpeed.Value;
        animSpeedNormalized = CurrentSpeed.Value / maxMoveSpeed;
        transform.position += moveVector * (moveSpeed * deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, deltaTime * rotationSpeed);
    }

    public override void LateTick(float deltaTime)
    {
        animator.SetFloat(MoveAnimSpeed, moveMagnitude * Mathf.Lerp(defaultMoveAnimSpeed, maxMoveAnimSpeed, animSpeedNormalized));
        animator.SetFloat(MoveSpeed, moveSpeed);
    }
}

public interface IMovementComponent
{
    MovementComponent GetMovementComponent();
}
using System;

public class EStateMachine<TEnum> where TEnum : Enum
{
    private TEnum CurrentState { get; set; }

    public EStateMachine(TEnum currentState)
    {
        CurrentState = currentState;
    }

    public void ChangeState(TEnum state)
    {
        CurrentState = state;
    }

    public bool IsState(TEnum state)
    {
        return CurrentState.Equals(state);
    }

    public TEnum GetState()
    {
        return CurrentState;
    }
}

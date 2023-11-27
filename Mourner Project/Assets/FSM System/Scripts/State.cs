public abstract class State
{
    protected FSM fSM;

    public State(FSM fSM)
    { this.fSM = fSM; }

    public abstract void EnterState();
    public abstract void FixedUpdateState();
    public abstract void UpdateState();
    public abstract void ExitState();
}

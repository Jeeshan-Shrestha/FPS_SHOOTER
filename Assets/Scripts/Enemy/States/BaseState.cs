public abstract class BaseState
{

    public Enemy enemy;
    public StateMachine stateMachine;
    public abstract void Enter();
    public abstract void Process();
    public abstract void Exit();

}
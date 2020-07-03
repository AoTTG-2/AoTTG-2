namespace StateManager
{
    public abstract class State<T> where T : State<T>
    {
        protected readonly StateManager<T> StateManager;

        protected State(StateManager<T> stateManager)
        {
            StateManager = stateManager;
            stateManager.Register((T) this);
        }
    }
}
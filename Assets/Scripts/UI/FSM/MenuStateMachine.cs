using VContainer;

public class MenuStateMachine
{
    private readonly IObjectResolver _resolver;
    private IMenuState _currentState;

    [Inject]
    public MenuStateMachine(IObjectResolver resolver)
    {
        _resolver = resolver;
    }

    public void Enter<T>() where T : IMenuState
    {
        _currentState?.Exit();
        _currentState = _resolver.Resolve<T>();
        _currentState.Enter();
    }
}
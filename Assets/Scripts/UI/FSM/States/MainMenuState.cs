using VContainer;

public class MainMenuState : IMenuState
{
    private readonly UIService _uiService;

    [Inject]
    public MainMenuState(UIService uiService)
    {
        _uiService = uiService;
    }

    public void Enter() => _uiService.ShowMenuPanel<MainMenuPanel>();
    public void Exit() { }
}
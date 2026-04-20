using VContainer;

public class SettingMenuState : IMenuState
{
    private readonly UIService _uiService;

    [Inject]
    public SettingMenuState(UIService uiService)
    {
        _uiService = uiService;
    }

    public void Enter() => _uiService.ShowMenuPanel<SettingMenuPanel>();
    public void Exit() { }
}
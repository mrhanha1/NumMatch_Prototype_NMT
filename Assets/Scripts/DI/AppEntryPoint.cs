using VContainer;
using VContainer.Unity;

public class AppEntryPoint : IStartable
{
    private readonly MenuStateMachine _fsm;
    private readonly UIService _uiService;
    private readonly MainMenuPanel _mainMenuPanel;
    private readonly SettingMenuPanel _settingMenuPanel;
    private readonly GameplayPanel _gameplayPanel;
    private readonly AudioService _audioService;
    private readonly AudioConfig _audioConfig;

    [Inject]
    public AppEntryPoint(MenuStateMachine fsm, UIService uiService,
        MainMenuPanel mainMenuPanel, SettingMenuPanel settingMenuPanel,
        GameplayPanel gameplayPanel, AudioService audioService, AudioConfig audioConfig)
    {
        _fsm = fsm;
        _uiService = uiService;
        _mainMenuPanel = mainMenuPanel;
        _settingMenuPanel = settingMenuPanel;
        _gameplayPanel = gameplayPanel;
        _audioService = audioService;
        _audioConfig = audioConfig;
    }

    public void Start()
    {
        _uiService.RegisterMenuPanel(_mainMenuPanel);
        _uiService.RegisterMenuPanel(_settingMenuPanel);

        foreach (var entry in _audioConfig.entries)
            _audioService.RegisterClip(entry.key, entry.clip);

        _fsm.Enter<MainMenuState>();
    }
}
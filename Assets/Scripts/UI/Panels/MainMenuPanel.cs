using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenuPanel : BasePanel
{
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnSetting;

    private MenuStateMachine _fsm;
    private UIService _uiService;
    private GameplayPanel _gameplayPanel;

    [Inject]
    public void Construct(MenuStateMachine fsm, UIService uiService, GameplayPanel gameplayPanel)
    {
        _fsm = fsm;
        _uiService = uiService;
        _gameplayPanel = gameplayPanel;
    }

    protected override void Awake()
    {
        base.Awake();
        btnPlay.onClick.AddListener(() => _uiService.PushPanel(_gameplayPanel));
        btnSetting.onClick.AddListener(() => _fsm.Enter<SettingMenuState>());
    }
}
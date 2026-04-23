using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenuPanel : BasePanel
{
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnGemMode;
    [SerializeField] private Button btnSetting;

    private MenuStateMachine _fsm;
    private UIService _uiService;
    private GameplayPanel _gameplayPanel;
    private GameController _controller;

    [Inject]
    public void Construct(MenuStateMachine fsm, UIService uiService, GameplayPanel gameplayPanel, GameController gameController)
    {
        _fsm = fsm;
        _uiService = uiService;
        _gameplayPanel = gameplayPanel;
        _controller = gameController;
    }

    protected override void Awake()
    {
        base.Awake();
        btnPlay.onClick.AddListener(() =>
        {
            _controller.SetGemMode(false);
            _uiService.PushPanel(_gameplayPanel);
        });
        btnSetting.onClick.AddListener(() => _fsm.Enter<SettingMenuState>());
        btnGemMode.onClick.AddListener(() =>
        {
           _controller.SetGemMode(true);
            _uiService.PushPanel(_gameplayPanel);
        });
    }
}
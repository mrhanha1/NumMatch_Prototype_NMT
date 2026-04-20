using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LosePopup : BasePopup
{
    [SerializeField] private Button replayButton;
    [SerializeField] private Button homeButton;

    private GameController _gameController;
    private UIService _uiService;
    private MenuStateMachine _fsm;

    [Inject]
    public void Construct(GameController gameController, UIService uiService, MenuStateMachine fsm)
    {
        _gameController = gameController;
        _uiService = uiService;
        _fsm = fsm;
    }

    private new void Awake()
    {
        replayButton.onClick.AddListener(OnReplay);
        homeButton.onClick.AddListener(OnHome);
    }

    private void OnReplay()
    {
        _uiService.HideTopPopup();
        _gameController.Restart();
    }

    private void OnHome()
    {
        _uiService.HideTopPopup();
        _uiService.PopPanel();
        _fsm.Enter<MainMenuState>();
    }
}
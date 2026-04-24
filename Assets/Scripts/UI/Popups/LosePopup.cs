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
    private AudioService _audioService;

    [Inject]
    public void Construct(GameController gameController, UIService uiService, MenuStateMachine fsm, AudioService audioService)
    {
        _gameController = gameController;
        _uiService = uiService;
        _fsm = fsm;
        _audioService = audioService;
    }

    private new void Awake()
    {
        replayButton.onClick.AddListener(OnReplay);
        homeButton.onClick.AddListener(OnHome);
    }

    private void OnReplay()
    {
        _audioService.PlaySFX("pop");
        _uiService.HideTopPopup();
        _gameController.Restart();
    }

    private void OnHome()
    {
        _audioService.PlaySFX("pop");
        _uiService.HideTopPopup();
        _uiService.PopPanel();
        _fsm.Enter<MainMenuState>();
    }
}
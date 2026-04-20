using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GameplayPanel : BasePanel
{
    private GameController _gameController;
    private GameResultService _gameResultService;
    private UIService _uiService;


    [SerializeField] private Button backButton;
    [SerializeField] private Button forceWinButton;
    [SerializeField] private Button forceLoseButton;

    [Inject]
    public void Construct(GameController gameController, UIService uIService, GameResultService gameResultService)
    {
        _gameController = gameController;
        _uiService = uIService;
        _gameResultService = gameResultService;
    }

    public override void Show()
    {
        base.Show();
        _gameController.StartGame();
    }
    private new void Awake()
    {
        forceWinButton.onClick.AddListener(() =>  _gameResultService.TriggerWin());
        forceLoseButton.onClick.AddListener(() => _gameResultService.TriggerLose());
        backButton.onClick.AddListener(() => _uiService.PopPanel());
    }
}
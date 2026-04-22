using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GameplayPanel : BasePanel
{
    private GameController _gameController;
    private GameResultService _gameResultService;
    private UIService _uiService;
    private GameSession _session;


    [SerializeField] private Button backButton;
    [SerializeField] private Button forceWinButton;
    [SerializeField] private Button forceLoseButton;
    [SerializeField] private Button addNumberButton;
    [SerializeField] private Text stageText;
    [SerializeField] private Text AddNumberCountText;

    [Inject]
    public void Construct(GameController gameController, UIService uIService, GameResultService gameResultService, GameSession gameSession)
    {
        _gameController = gameController;
        _uiService = uIService;
        _gameResultService = gameResultService;
        _session = gameSession;
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
        addNumberButton.onClick.AddListener(() => _gameController.AddNumbers());
    }
    private void OnEnable()
    {
        _session.OnStageChanged += UpdateStageText;
        _session.OnAddNumberCountChanged += UpdateAddNumberCountText;
    }
    private void OnDisable()
    {
        _session.OnStageChanged -= UpdateStageText;
        _session.OnAddNumberCountChanged -= UpdateAddNumberCountText;
    }
    private void UpdateStageText() => stageText.text = $"Stage: {_session.Stage}";
    private void UpdateAddNumberCountText() => AddNumberCountText.text = $"{_session.AddNumberCount}";
}
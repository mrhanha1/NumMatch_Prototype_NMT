using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GameplayPanel : BasePanel
{
    private GameController _gameController;
    private GameResultService _gameResultService;
    private UIService _uiService;
    private GameSession _session;
    private BoardCollapser _collapser;
    private AudioService _audioService;

    [SerializeField] private Button backButton;
    [SerializeField] private Button forceWinButton;
    [SerializeField] private Button forceLoseButton;
    [SerializeField] private Button addNumberButton;
    [SerializeField] private Text stageText;
    [SerializeField] private Text AddNumberCountText;

    [SerializeField] private Button btnTestSolver;

    [Header("Gem Require HUD")]
    [SerializeField] private GemRequireView gemRequireView;

    [Inject]
    public void Construct(GameController gameController, UIService uIService,
        GameResultService gameResultService, GameSession gameSession,
        BoardCollapser collapser, AudioService audioService)
    {
        _gameController = gameController;
        _uiService = uIService;
        _gameResultService = gameResultService;
        _session = gameSession;
        _collapser = collapser;
        _audioService = audioService;
    }

    public override void Show()
    {
        base.Show();
        _gameController.StartGame();
        gemRequireView.Build();
    }

    private new void Awake()
    {
        forceWinButton.onClick.AddListener(() =>
        {
            for (int i = 0; i < _session.Board.GetLength(0); i++)
                for (int j = 0; j < _session.Board.GetLength(1); j++)
                    _session.Board[i, j].IsActive = false;
            _collapser.Collapse();
            _gameResultService.CheckResult();
        });
        forceLoseButton.onClick.AddListener(() => _gameResultService.TriggerLose());
        backButton.onClick.AddListener(() =>
            {
                _uiService.PopPanel();
                _audioService.PlaySFX("pop");
            });

        addNumberButton.onClick.AddListener(() => {
            _gameController.AddNumbers();
            _audioService.PlaySFX("pop");
        });
        btnTestSolver.onClick.AddListener(() =>
        {
            _audioService.PlaySFX("pop");
            CellModel[,] testBoard = _session.Board.Clone() as CellModel[,];
            RunSolver(testBoard);
        });
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

    public void RunSolver(CellModel[,] board)
    {
        List<string> solutions = FiveSolver.Solve(_session.cellList);

        if (solutions.Count == 0)
        {
            Debug.Log("Không tìm được lời giải nào.");
            return;
        }

        for (int i = 0; i < solutions.Count; i++)
            Debug.Log($"Lời giải {i + 1}: {solutions[i]}");

    }
}
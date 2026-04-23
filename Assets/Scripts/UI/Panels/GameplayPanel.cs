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
    private GameConfig _gameConfig;

    [SerializeField] private Button backButton;
    [SerializeField] private Button forceWinButton;
    [SerializeField] private Button forceLoseButton;
    [SerializeField] private Button addNumberButton;
    [SerializeField] private Text stageText;
    [SerializeField] private Text AddNumberCountText;

    [Header("Gem Require HUD")]
    [SerializeField] private Transform gemRequireGrid;
    [SerializeField] private GemRequireItemView gemRequireItemPrefab;

    private readonly List<GemRequireItemView> _gemItems = new();

    [Inject]
    public void Construct(GameController gameController, UIService uIService,
        GameResultService gameResultService, GameSession gameSession,
        BoardCollapser collapser, GameConfig gameConfig)
    {
        _gameController = gameController;
        _uiService = uIService;
        _gameResultService = gameResultService;
        _session = gameSession;
        _collapser = collapser;
        _gameConfig = gameConfig;
    }

    public override void Show()
    {
        base.Show();
        _gameController.StartGame();
        BuildGemRequireUI();
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
        backButton.onClick.AddListener(() => _uiService.PopPanel());
        addNumberButton.onClick.AddListener(() => _gameController.AddNumbers());
    }

    private void OnEnable()
    {
        _session.OnStageChanged += UpdateStageText;
        _session.OnAddNumberCountChanged += UpdateAddNumberCountText;
        _session.OnGemRequiredChanged += RefreshGemRequireUI;
    }

    private void OnDisable()
    {
        _session.OnStageChanged -= UpdateStageText;
        _session.OnAddNumberCountChanged -= UpdateAddNumberCountText;
        _session.OnGemRequiredChanged -= RefreshGemRequireUI;
    }

    private void UpdateStageText() => stageText.text = $"Stage: {_session.Stage}";
    private void UpdateAddNumberCountText() => AddNumberCountText.text = $"{_session.AddNumberCount}";

    /// <summary>Destroys old gem items and rebuilds the grid from GemRequired.</summary>
    private void BuildGemRequireUI()
    {
        foreach (var item in _gemItems)
            if (item != null) Destroy(item.gameObject);
        _gemItems.Clear();

        if (!_session.GemMode || _session.GemRequired == null || gemRequireGrid == null || gemRequireItemPrefab == null)
            return;

        foreach (var kvp in _session.GemRequired)
        {
            int gemType = kvp.Key;
            int count = kvp.Value;
            var item = Instantiate(gemRequireItemPrefab, gemRequireGrid);

            Sprite sprite = (gemType < _gameConfig.gemSprites.Count) ? _gameConfig.gemSprites[gemType] : null;
            item.Bind(sprite, count);
            _gemItems.Add(item);
        }
    }

    /// <summary>Updates count text on each gem item without rebuilding the list.</summary>
    private void RefreshGemRequireUI()
    {
        if (!_session.GemMode || _session.GemRequired == null) return;

        int index = 0;
        foreach (var kvp in _session.GemRequired)
        {
            if (index < _gemItems.Count)
                _gemItems[index].UpdateCount(kvp.Value);
            index++;
        }
    }
}
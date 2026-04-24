using VContainer;

public class GameController
{
    private readonly GameSession _session;
    private readonly GridView _gridView;

    [Inject]
    public GameController(GameSession session, GridView gridView)
    {
        _session = session;
        _gridView = gridView;
    }

    public void StartGame()
    {
        string input = StageGenerator.GenerateStage(32, targetPairCount: 3);
        _session.Stage = 1;
        _session.Reset(input);
        _gridView.BuildGrid(_session.Board);
    }

    public void SetGemMode(bool isGemMode)
    {
        _session.GemMode = isGemMode;
        if (isGemMode)
        {
            _session.GemRequired = new() { { 1, 5 }, { 2, 5 }, { 3, 5 } };
        }
    }

    public void NextStage()
    {
        string input = string.Empty;
        _session.Stage++;
        _session.TriggerStageChanged();
        _session.AddNumberCount = 5;
        _session.TriggerAddNumberCountChanged();
        if (_session.Stage == 1) input = StageGenerator.GenerateStage(32, 3);
        else if (_session.Stage == 2) input = StageGenerator.GenerateStage(32, 2);
        else if (_session.Stage >= 3) input = StageGenerator.GenerateStage(32, 1);
        _session.Reset(input);
        _gridView.BuildGrid(_session.Board);
    }

    public void Restart() => StartGame();

    public void AddNumbers()
    {
        AddNumber.Execute(_session, _gridView);
        _gridView.BuildGrid(_session.Board);
        _session.TriggerAddNumberCountChanged();
    }
}
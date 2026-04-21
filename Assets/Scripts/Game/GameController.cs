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
        string input = StageGenerator.GenerateStage(32, targetPairCount:1);
        _session.Stage = 1;
        _session.Reset(input);
        _gridView.BuildGrid(_session.Board);
    }
    public void NextStage()
    {
        string input = string.Empty;
        _session.Stage++;
        if (_session.Stage == 1) input = StageGenerator.GenerateStage(32, 3);
        else if (_session.Stage == 2) input = StageGenerator.GenerateStage(32, 2);
        else if (_session.Stage >= 3) input = StageGenerator.GenerateStage(32, 1);
        _session.Reset(input);
        _gridView.BuildGrid(_session.Board);
    }

    public void Restart() => StartGame();
    public void AddNumbers()
    {
        if (_session.AddNumberCount <= 0) return;

        _session.AddNumberCount--;
        string activeCells = AddNumber.FindActivedCells(_session.Board);

        // Tìm vị trí đầu tiên chưa có value
        for (int r = 0; r < _session.Board.GetLength(0); r++)
            for (int c = 0; c < _session.Board.GetLength(1); c++)
                if (_session.Board[r, c].Value == 0)
                {
                    _session.InsertNumber(_session.Board, activeCells, r, c);
                    _gridView.BuildGrid(_session.Board);
                    return;
                }
    }
    public void CheckWinCondition()
    {
        if (_session.IsAllCleared())
            NextStage();
    }
}
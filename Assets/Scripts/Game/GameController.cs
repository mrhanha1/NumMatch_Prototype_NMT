using System;
using System.Collections.Generic;
using System.Text;
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
        _session.Reset(input);
        _gridView.BuildGrid(_session.Board);
    }

    public void Restart() => StartGame();
}
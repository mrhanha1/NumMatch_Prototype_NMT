using VContainer;

public class GameResultService
{
    private readonly GameSession _session;
    private readonly UIService _uiService;
    private readonly WinPopup _winPopup;
    private readonly LosePopup _losePopup;

    [Inject]
    public GameResultService(GameSession session, UIService uiService, WinPopup winPopup, LosePopup losePopup)
    {
        _session = session;
        _uiService = uiService;
        _winPopup = winPopup;
        _losePopup = losePopup;
    }

    public void CheckResult()
    {
        var activeCells = AddNumber.ParseNumberString(AddNumber.FindActivedCells(_session.Board));
        var pairs = StageGenerator.FindAllPairs(activeCells);

        if (_session.GemMode)
        {
            if (IsGemComplete()) TriggerWin();
            else if (_session.AddNumberCount == 0 && pairs.Count == 0) TriggerLose();
        }
        else
        {
            if (_session.AddNumberCount == 0 && pairs.Count == 0) TriggerWin();
        }
    }
    private bool IsGemComplete()
    {
        foreach (var kvp in _session.GemRequired)
            if (_session.GemCollected[kvp.Key] < kvp.Value) return false;
        return true;
    }

    public void TriggerWin()
    {
        _session.IsWin = true;
        _uiService.ShowPopup(_winPopup);
    }

    public void TriggerLose()
    {
        _session.IsWin = false;
        _uiService.ShowPopup(_losePopup);
    }
}
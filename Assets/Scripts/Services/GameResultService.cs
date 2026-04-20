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
        if (_session.IsAllCleared())
            TriggerWin();
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
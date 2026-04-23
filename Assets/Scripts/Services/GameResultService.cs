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

    /// <summary>
    /// Call after every match or board change to evaluate game state.
    ///
    /// GemMode:
    ///   Win  — all GemRequired fully collected (each value <= 0)
    ///   Lose — AddNumberCount <= 0 AND no valid moves AND gems NOT all collected
    ///
    /// Non-GemMode (and GemMode path for complete):
    ///   Complete — AddNumberCount <= 0 AND no valid moves → show WinPopup
    /// </summary>
    public void CheckResult()
    {
        if (_session.GemMode)
        {
            if (_session.AreAllGemsCollected())
            {
                TriggerWin();
                return;
            }

            if (_session.AddNumberCount <= 0 && !_session.HasValidMoves())
            {
                TriggerLose();
                return;
            }
        }
        else
        {
            if (_session.AddNumberCount <= 0 && !_session.HasValidMoves())
            {
                TriggerComplete();
            }
        }
    }

    public void TriggerWin()
    {
        _uiService.ShowPopup(_winPopup);
    }

    public void TriggerLose()
    {
        _uiService.ShowPopup(_losePopup);
    }

    /// <summary>No valid moves left and add-number exhausted → show WinPopup as stage complete.</summary>
    public void TriggerComplete()
    {
        _uiService.ShowPopup(_winPopup);
    }
}
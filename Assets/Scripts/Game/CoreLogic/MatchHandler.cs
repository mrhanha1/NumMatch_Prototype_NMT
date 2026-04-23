using System;
using VContainer;

public class MatchHandler
{
    private readonly GameSession _session;
    private readonly GameResultService _resultService;
    private readonly BoardCollapser _collapser;
    private readonly AudioService _audioService;

    public event Action<CellModel, CellModel> OnMatchSuccess;
    public event Action OnMatchFail;

    [Inject]
    public MatchHandler(GameSession session, GameResultService resultService, BoardCollapser collapser, AudioService audioService)
    {
        _session = session;
        _resultService = resultService;
        _collapser = collapser;
        _audioService = audioService;
    }

    public bool TryMatch(CellView a, CellView b)
    {
        if (!MatchRule.IsMatch(a.Model, b.Model) || !PathValidator.HasValidPath(_session.Board, a.Model, b.Model))
        {
            OnMatchFail?.Invoke();
            return false;
        }

        a.Model.IsActive = false;
        b.Model.IsActive = false;
        _session.Score += 2;

        if (_session.GemMode)
        {
            if (a.Model.GemType > 0)
            {
                _session.GemRequired[a.Model.GemType] = Math.Max(0, _session.GemRequired[a.Model.GemType] - 1);
                _session.TriggerGemRequiredChanged();
            }
            if (b.Model.GemType > 0)
            {
                _session.GemRequired[b.Model.GemType] = Math.Max(0, _session.GemRequired[b.Model.GemType] - 1);
                _session.TriggerGemRequiredChanged();
            }
        }

        _audioService.PlaySFX("pair");
        OnMatchSuccess?.Invoke(a.Model, b.Model);

        _collapser.Collapse();
        _resultService.CheckResult();
        return true;
    }
}
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SettingMenuPanel : BasePanel
{
    [SerializeField] private Button btnBack;

    private MenuStateMachine _fsm;
    private AudioService _audioService;

    [Inject]
    public void Construct(MenuStateMachine fsm,AudioService audioService)
    {
        _audioService = audioService;
        _fsm = fsm;
    }

    protected override void Awake()
    {
        base.Awake();
        btnBack.onClick.AddListener(() =>
        {
            _audioService.PlaySFX("pop");
            _fsm.Enter<MainMenuState>();
        });
    }
}
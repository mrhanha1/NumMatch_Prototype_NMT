using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SettingMenuPanel : BasePanel
{
    [SerializeField] private Button btnBack;

    private MenuStateMachine _fsm;

    [Inject]
    public void Construct(MenuStateMachine fsm) => _fsm = fsm;

    protected override void Awake()
    {
        base.Awake();
        btnBack.onClick.AddListener(() => _fsm.Enter<MainMenuState>());
    }
}
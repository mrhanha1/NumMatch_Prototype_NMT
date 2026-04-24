using UnityEngine;
using VContainer;
using VContainer.Unity;

public class AppLifetimeScope : LifetimeScope
{
    [SerializeField] private MainMenuPanel mainMenuPanel;
    [SerializeField] private SettingMenuPanel settingMenuPanel;
    [SerializeField] private GameplayPanel gameplayPanel;
    [SerializeField] private GridView gridView;
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private WinPopup winPopup;
    [SerializeField] private LosePopup losePopup;
    [SerializeField] private GemRequireView gemRequireView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(mainMenuPanel);
        builder.RegisterComponent(settingMenuPanel);
        builder.RegisterComponent(gameplayPanel);
        builder.RegisterComponent(gridView);
        builder.RegisterInstance(gameConfig);
        builder.RegisterInstance(audioConfig);
        builder.RegisterComponent(winPopup);
        builder.RegisterComponent(losePopup);
        builder.RegisterComponent(gemRequireView);

        builder.Register<UIService>(Lifetime.Singleton);
        builder.Register<MenuStateMachine>(Lifetime.Singleton);
        builder.Register<MainMenuState>(Lifetime.Singleton);
        builder.Register<SettingMenuState>(Lifetime.Singleton);
        builder.Register<GameSession>(Lifetime.Singleton);
        builder.Register<GameController>(Lifetime.Singleton);
        builder.Register<AudioService>(Lifetime.Singleton);
        builder.Register<VFXService>(Lifetime.Singleton);
        builder.Register<GameResultService>(Lifetime.Singleton);
        builder.Register<MatchHandler>(Lifetime.Singleton);
        builder.Register<BoardCollapser>(Lifetime.Singleton);
        builder.Register<CellSelectionHandler>(Lifetime.Singleton);

        builder.RegisterEntryPoint<AppEntryPoint>();
    }
}
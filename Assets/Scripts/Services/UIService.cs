using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class UIService
{
    private readonly Dictionary<System.Type, BasePanel> _menuPanels = new();
    private readonly Stack<BasePanel> _gameplayStack = new();
    private readonly Stack<BasePopup> _popupStack = new();

    private BasePanel _currentMenuPanel;

    public void RegisterMenuPanel<T>(T panel) where T : BasePanel
        => _menuPanels[typeof(T)] = panel;

    public void ShowMenuPanel<T>() where T : BasePanel
    {
        _currentMenuPanel?.Hide();
        if (_menuPanels.TryGetValue(typeof(T), out var panel))
        {
            _currentMenuPanel = panel;
            panel.Show();
        }
    }

    public void PushPanel(BasePanel panel)
    {
        _gameplayStack.TryPeek(out var top);
        top?.Hide();
        _gameplayStack.Push(panel);
        panel.Show();
    }

    public void PopPanel()
    {
        if (_gameplayStack.Count == 0) return;
        _gameplayStack.Pop().Hide();
        if (_gameplayStack.TryPeek(out var prev)) prev.Show();
    }

    public void ShowPopup<T>(T popup) where T : BasePopup
    {
        _popupStack.Push(popup);
        popup.Show();
    }

    public void HideTopPopup()
    {
        if (_popupStack.Count == 0) return;
        _popupStack.Pop().Hide();
    }
}
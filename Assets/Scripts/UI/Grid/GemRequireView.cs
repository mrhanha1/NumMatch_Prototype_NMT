using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class GemRequireView : MonoBehaviour
{
    [SerializeField] private Transform gemRequireGrid;
    [SerializeField] private GemRequireItem gemRequireItemPrefab;

    private readonly List<GemRequireItem> _gemItems = new();
    private GameSession _session;
    private GameConfig _gameConfig;

    [Inject]
    public void Construct(GameSession session, GameConfig gameConfig)
    {
        _session = session;
        _gameConfig = gameConfig;
    }

    private void OnEnable()
    {
        _session.OnGemRequiredChanged += Refresh;
    }

    private void OnDisable()
    {
        _session.OnGemRequiredChanged -= Refresh;
    }

    public void Build()
    {
        Clear();

        if (!_session.GemMode || _session.GemRequired == null) return;

        foreach (var kvp in _session.GemRequired)
        {
            var item = Instantiate(gemRequireItemPrefab, gemRequireGrid);
            Sprite sprite = _gameConfig.gemSprites[kvp.Key];
            item.Bind(sprite, kvp.Value);
            _gemItems.Add(item);
        }
    }

    private void Refresh()
    {
        if (!_session.GemMode || _session.GemRequired == null) return;

        int index = 0;
        foreach (var kvp in _session.GemRequired)
        {
            if (index < _gemItems.Count)
                _gemItems[index].UpdateCount(kvp.Value);
            index++;
        }
    }

    private void Clear()
    {
        foreach (var item in _gemItems)
            if (item != null) Destroy(item.gameObject);
        _gemItems.Clear();
    }
}
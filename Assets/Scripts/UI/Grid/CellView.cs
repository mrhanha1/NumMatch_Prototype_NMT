using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CellView : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private Image numImg;
    [SerializeField] private GameObject matchedOverlay;
    [SerializeField] private GameObject selectedHighlight;
    [SerializeField] private Image gemBG;

    public CellModel Model { get; private set; }

    private AudioService _audioService;
    private CellSelectionHandler _selectionHandler;
    private GameConfig _config;

    [Inject]
    public void Construct(AudioService audioService, CellSelectionHandler selectionHandler, GameConfig gameConfig)
    {
        _audioService = audioService;
        _selectionHandler = selectionHandler;
        _config = gameConfig;
    }

    public void Init(CellModel model)
    {
        Model = model;
        Refresh();
        btn.onClick.AddListener(OnClick);
    }

    public void Refresh()
    {
        matchedOverlay.SetActive(!Model.IsActive);
        selectedHighlight.SetActive(Model.IsSelected);
        if (Model.GemType==0) gemBG.gameObject.SetActive(false);
        else SetGemType(Model.GemType);

        numImg.gameObject.SetActive(Model.Value > 0);
        if (Model.Value > 0) numImg.sprite = _config.numberSprites[Model.Value];
    }

    public void SetSelected(bool selected)
    {
        Model.IsSelected = selected;
        selectedHighlight.SetActive(selected);
        _audioService.PlaySFX("choose");
    }
    public void SetGemType(int type)
    {
        if (type <= 0) return;
        gemBG.gameObject.SetActive(true);
        gemBG.sprite = _config.gemSprites[type];
        Model.GemType = type;
    }
    private void OnClick()
    {
        if (!Model.IsActive) return;
        _selectionHandler.OnCellClicked(this);
    }
}
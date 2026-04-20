using UnityEngine;
using VContainer;

/// <summary>
/// Qu?n lý vi?c ch?n cell: l?n 1 ? highlight, l?n 2 ? th? match.
/// Inject vào CellView qua resolver.
/// </summary>
public class CellSelectionHandler
{
    private readonly MatchHandler _matchHandler;

    private CellView _selected;

    [Inject]
    public CellSelectionHandler(MatchHandler matchHandler)
    {
        _matchHandler = matchHandler;
    }

    public void OnCellClicked(CellView cellView)
    {

        if (_selected == null) // neu chua co cell nao duoc chon thi chon cell vua bam
        {
            _selected = cellView;
            cellView.SetSelected(true);
            Debug.Log($"Selected cell at ({cellView.Model.Row}, {cellView.Model.Col}) with value {cellView.Model.Value}");
            return;
        }

        if (_selected == cellView)// neu bam lai cell da chon thi bo chon
        {
            cellView.SetSelected(false);
            _selected = null;
            Debug.Log($"Deselected cell at ({cellView.Model.Row}, {cellView.Model.Col})");
            return;
        }
        //neu da co cell duoc chon va bam vao cell khac
        //Debug.Log($"Try match cell at ({_selected.Model.Row}, {_selected.Model.Col}) with cell at ({cellView.Model.Row}, {cellView.Model.Col})");
        CellView prev = _selected;
        _selected = null;
        bool matched = _matchHandler.TryMatch(prev, cellView);

        if (matched)
        {
            prev.Refresh();
            cellView.Refresh();
            //Debug.Log($"Matched cell");
        }
        prev.SetSelected(false);
        cellView.SetSelected(false);
    }
}
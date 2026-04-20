using System.Collections.Generic;
using VContainer;

public class BoardCollapser
{
    private readonly GameSession _session;
    private readonly GridView _gridView;

    [Inject]
    public BoardCollapser(GameSession session, GridView gridView)
    {
        _session = session;
        _gridView = gridView;
    }

    public bool Collapse()
    {
        var board = _session.Board;
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        var activeRows = new List<CellModel[]>();
        for (int r = 0; r < rows; r++)
        {
            bool anyActive = false;
            for (int c = 0; c < cols; c++)
                if (board[r, c].IsActive) { anyActive = true; break; }
            if (anyActive)
            {
                var row = new CellModel[cols];
                for (int c = 0; c < cols; c++) row[c] = board[r, c];
                activeRows.Add(row);
            }
        }

        if (activeRows.Count == rows) return false;

        var newBoard = new CellModel[activeRows.Count, cols];
        for (int r = 0; r < activeRows.Count; r++)
            for (int c = 0; c < cols; c++)
            {
                newBoard[r, c] = activeRows[r][c];
                newBoard[r, c].SetPosition(r, c);
            }

        _session.SetBoard(newBoard);
        _gridView.BuildGrid(_session.Board);
        return true;
    }
}
using System.Collections.Generic;
using VContainer;

public class BoardCollapser
{
    private readonly GameSession _session;
    private readonly GridView _gridView;
    private readonly AudioService _audioService;
    private readonly GameController _gameController;

    [Inject]
    public BoardCollapser(GameSession session, GridView gridView, AudioService audioService, GameController gameController)
    {
        _session = session;
        _gridView = gridView;
        _audioService = audioService;
        _gameController = gameController;
    }

    public bool Collapse()
    {
        var board = _session.Board;
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        // Find rows to remove (all cells inactive = fully matched)
        var rowsToRemove = new HashSet<int>();
        for (int r = 0; r < rows; r++)
        {
            bool allInactive = true;
            for (int c = 0; c < cols; c++)
                if (board[r, c].IsActive) { allInactive = false; break; }
            if (allInactive) rowsToRemove.Add(r);
        }

        if (rowsToRemove.Count == 0) return false;

        _audioService.PlaySFX("clear");

        // Shift active rows up to fill removed rows
        int writeRow = 0;
        for (int r = 0; r < rows; r++)
        {
            if (rowsToRemove.Contains(r)) continue;
            if (writeRow != r)
                for (int c = 0; c < cols; c++)
                {
                    board[writeRow, c].Value = board[r, c].Value;
                    board[writeRow, c].IsActive = board[r, c].IsActive;
                    board[writeRow, c].GemType = board[r, c].GemType;
                }
            writeRow++;
        }

        // Reset bottom rows that were duplicated
        for (int r = rows - rowsToRemove.Count; r < rows; r++)
            for (int c = 0; c < cols; c++)
            {
                board[r, c].Value = 0;
                board[r, c].IsActive = true;
                board[r, c].GemType = 0;
            }

        _gridView.BuildGrid(board);

        // Auto advance to next stage when top 3 rows are all empty (board cleared)
        if (_session.IsBoardCleared())
        {
            _gameController.NextStage();
        }

        return true;
    }
}
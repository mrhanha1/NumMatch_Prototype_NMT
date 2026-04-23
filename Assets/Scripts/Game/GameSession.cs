
using System.Collections.Generic;

public class GameSession
{
    public bool GemMode { get; set; } = false;
    public Dictionary<int, int> GemRequired { get; set; }
    public int Score { get; set; }
    public int Stage { get; set; }
    public int AddNumberCount { get; set; }
    public CellModel[,] Board { get; private set; }

    public event System.Action OnStageChanged;
    public event System.Action OnAddNumberCountChanged;
    public event System.Action OnGemRequiredChanged;

    public void TriggerStageChanged() => OnStageChanged?.Invoke();
    public void TriggerAddNumberCountChanged() => OnAddNumberCountChanged?.Invoke();
    public void TriggerGemRequiredChanged() => OnGemRequiredChanged?.Invoke();

    private readonly GameConfig _config;
    public GameSession(GameConfig config)
    {
        _config = config;
    }

    public void Reset(string numberString)
    {
        Score = 0;
        AddNumberCount = 5;
        int rows = _config.row;
        int cols = _config.column;
        Board = new CellModel[rows, cols];

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                Board[r, c] = new CellModel(r, c, 0);

        InsertNumber(Board, numberString, 0, 0);
        OnStageChanged?.Invoke();
        OnAddNumberCountChanged?.Invoke();
    }

    public void InsertNumber(CellModel[,] board, string numberString, int startRow = 0, int startCol = 0)
    {
        var numList = AddNumber.ParseNumberString(numberString);
        var gemList = GemGenerator.GenerateGemList(numberString, GemRequired);
        int rows = _config.row;
        int cols = _config.column;
        int i = 0;

        for (int r = startRow; r < rows && i < numList.Count; r++)
        {
            int cStart = (r == startRow) ? startCol : 0;
            for (int c = cStart; c < cols && i < numList.Count; c++)
            {
                if (board[r, c].Value == 0 && numList[i] != 0)
                {
                    board[r, c].Value = numList[i];
                    if (GemMode) board[r, c].GemType = gemList[i];
                }
                i++;
            }
        }
    }

    public void SetBoard(CellModel[,] newBoard)
    {
        Board = newBoard;
    }

    /// <summary>Returns true when the top 3 rows have no cell with a value (board fully cleared).</summary>
    public bool IsBoardCleared()
    {
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < Board.GetLength(1); c++)
                if (Board[r, c].Value != 0) return false;
        return true;
    }

    /// <summary>Returns true when all gem types in GemRequired have been fully collected (count <= 0).</summary>
    public bool AreAllGemsCollected()
    {
        if (GemRequired == null) return true;
        foreach (var kvp in GemRequired)
            if (kvp.Value > 0) return false;
        return true;
    }

    public bool HasValidMoves()
    {
        int rows = Board.GetLength(0);
        int cols = Board.GetLength(1);

        for (int r1 = 0; r1 < rows; r1++)
        {
            for (int c1 = 0; c1 < cols; c1++)
            {
                CellModel cellA = Board[r1, c1];
                if (cellA.Value == 0) return false;
                if (!cellA.IsActive) continue;

                for (int r2 = r1; r2 < rows; r2++)
                {
                    int startCol = (r2 == r1) ? c1 : 0;
                    for (int c2 = startCol; c2 < cols; c2++) // fix: c2 < cols (was startCol < cols)
                    {
                        CellModel cellB = Board[r2, c2];
                        if (cellB.Value == 0) break;
                        if (!cellB.IsActive || cellB == cellA) continue;

                        if (MatchRule.IsMatch(cellA, cellB) && PathValidator.HasValidPath(Board, cellA, cellB))
                            return true;
                    }
                }
            }
        }

        return false;
    }
}
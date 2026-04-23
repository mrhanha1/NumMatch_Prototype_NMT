
using System.Collections.Generic;

public class GameSession
{
    public bool GemMode { get; set; } = false;
    public Dictionary<int, int> GemRequired { get; set; }
    public Dictionary<int, int> GemCollected { get; set; }
    public int Score { get; set; }
    public int Stage { get; set; }
    public int AddNumberCount { get; set; }
    public CellModel[,] Board { get; private set; }
    public bool IsWin { get; set; }
    public event System.Action OnStageChanged;
    public event System.Action OnAddNumberCountChanged;
    public void TriggerStageChanged() => OnStageChanged?.Invoke();
    public void TriggerAddNumberCountChanged() => OnAddNumberCountChanged?.Invoke();

    private readonly GameConfig _config;
    public GameSession(GameConfig config)
    {
        _config = config;
    }


    public void Reset(string numberString)
    {
        Score = 0;
        IsWin = false;
        AddNumberCount = 5;
        int rows = _config.row;
        int cols = _config.column;
        Board = new CellModel[rows, cols];

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                Board[r, c] = new CellModel(r,c,0);

        InsertNumber(Board,numberString,0,0);
        OnStageChanged?.Invoke();
        OnAddNumberCountChanged?.Invoke();
    }

    public void InsertNumber(CellModel[,] board, string numberString, int startRow = 0, int startCol = 0)
    {
        var numList = AddNumber.ParseNumberString(numberString);
        var gemList = GemGenerator.GenerateGemList(numberString, GemRequired, GemCollected);
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
    public bool IsAllCleared()
    {
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < Board.GetLength(1); c++)
                if (Board[r, c].IsActive) return false;
        return true;
    }
}
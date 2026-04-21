using System.Collections.Generic;

public class GameSession
{
    public int Score { get; set; }
    public int Stage { get; set; }
    public int AddNumberCount { get; set; }
    public CellModel[,] Board { get; private set; }
    public bool IsWin { get; set; }

    private readonly GameConfig _config;

    public GameSession(GameConfig config)
    {
        _config = config;
    }

    private List<int> ParseNumberString(string numberString)
    {
        var numbers = new List<int>();
        foreach (char c in numberString)
        {
            if (int.TryParse(c.ToString(), out int n))
                numbers.Add(n);
        }
        return numbers;
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
    }

    public void InsertNumber(CellModel[,] board, string numberString, int startRow = 0, int startCol = 0)
    {
        var numbers = ParseNumberString(numberString);
        int rows = _config.row;
        int cols = _config.column;
        int i = 0;

        for (int r = startRow; r < rows && i < numbers.Count; r++)
        {
            int cStart = (r == startRow) ? startCol : 0;
            for (int c = cStart; c < cols && i < numbers.Count; c++)
            {
                if (board[r, c].Value == 0 && numbers[i] != 0)
                    board[r, c].Value = numbers[i];
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
        int rows = _config.row;
        int cols = _config.column;
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                if (Board[r, c].IsActive) return false;
        return true;
    }
}
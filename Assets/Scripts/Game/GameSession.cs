public class GameSession
{
    public int Score { get; set; }
    public CellModel[,] Board { get; private set; }
    public bool IsWin { get; set; }

    private readonly GameConfig _config;

    public GameSession(GameConfig config)
    {
        _config = config;
    }

    public void Reset(string numberString = "")
    {
        Score = 0;
        IsWin = false;
        int rows = _config.row;
        int cols = _config.column;
        Board = new CellModel[rows, cols];

        int i = 0;
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
            {
                int val = (i < numberString.Length && int.TryParse(numberString[i].ToString(), out int n)) ? n : 0;
                Board[r, c] = new CellModel(r, c, val);
                i++;
            }
    }

    /// <summary>Dùng bởi BoardCollapser sau khi collapse.</summary>
    public void SetBoard(CellModel[,] newBoard)
    {
        Board = newBoard;
    }

    public bool IsAllCleared()
    {
        foreach (var cell in Board)
            if (cell.IsActive) return false;
        return true;
    }
}
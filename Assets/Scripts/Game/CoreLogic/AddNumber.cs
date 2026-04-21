public static class  AddNumber
{
    public static void AddNumberInMap(CellModel[,] board )
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (board[r, c].IsActive)
                {
                    board[r, c].Value += 1;
                }
            }
        }
    }
}
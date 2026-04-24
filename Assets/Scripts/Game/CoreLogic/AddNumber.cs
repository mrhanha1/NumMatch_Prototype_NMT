using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class AddNumber
{
    public static void Execute(GameSession session, GridView gridView)
    {
        if (session.AddNumberCount <= 0) return;

        session.AddNumberCount--;
        string activeCells = FindActivedCells(session.Board);

        // Find first empty slot and insert active cells there
        var (r, c) = FindFirstEmptyCell(session.Board);
        if (r >= 0) session.InsertNumber(activeCells, r, c);
    }
    public static (int row, int col) FindFirstEmptyCell(CellModel[,] board)
    {
        for (int r = 0; r < board.GetLength(0); r++)
            for (int c = 0; c < board.GetLength(1); c++)
                if (board[r, c].Value == 0)
                    return (r, c);
        return (-1, -1);
    }

    public static int FindLastRowWithValue(CellModel[,] board)
    {
        for (int r = board.GetLength(0) - 1; r >= 0; r--)
            for (int c = 0; c < board.GetLength(1); c++)
                if (board[r, c].Value != 0)
                    return r;
        return -1;
    }

    public static string FindActivedCells(CellModel[,] board)
    {
        StringBuilder sb = new();
        for (int r = 0; r < board.GetLength(0); r++)
            for (int c = 0; c < board.GetLength(1); c++)
            {
                if (board[r, c].IsActive)
                    sb.Append(board[r, c].Value);
                else if (board[r, c].Value == 0)
                    return sb.ToString();
            }
        return sb.ToString();
    }

    public static List<int> ParseNumberString(string numberString)
    {
        var numbers = new List<int>();
        foreach (char c in numberString)
        {
            if (int.TryParse(c.ToString(), out int n))
                numbers.Add(n);
        }
        return numbers;
    }
}
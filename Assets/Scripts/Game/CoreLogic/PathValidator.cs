
using UnityEngine;

public static class PathValidator
{
    public static bool HasValidPath(CellModel[,] board, CellModel a, CellModel b)
    {
        // --- Trường hợp 1: Cùng hàng, cùng cột, hoặc cùng đường chéo 45° ---
        int dx = b.Row - a.Row;
        int dy = b.Col - a.Col;

        bool sameLine = dx == 0 || dy == 0 || UnityEngine.Mathf.Abs(dx) == UnityEngine.Mathf.Abs(dy);
        if (sameLine)
        {
            int stepR = dx == 0 ? 0 : (dx/Mathf.Abs(dx));
            int stepC = dy == 0 ? 0 : (dy/Mathf.Abs(dy));

            int r = a.Row + stepR;
            int c = a.Col + stepC;

            while (r != b.Row || c != b.Col)
            {
                if (board[r, c].IsActive) return false;
                r += stepR;
                c += stepC;
            }

            return true;
        }

        // --- Trường hợp 2: Liền kề trên dải 1D row-major, không có cell active nào chắn giữa ---
        int cols = board.GetLength(1);
        int idxA = a.Row * cols + a.Col;
        int idxB = b.Row * cols + b.Col;
        int from = Mathf.Min(idxA, idxB);
        int to = Mathf.Max(idxA, idxB);

        for (int i = from + 1; i < to; i++)
        {
            if (board[i / cols, i % cols].IsActive) return false;
        }

        return true;
    }
}
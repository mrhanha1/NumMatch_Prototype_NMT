
using UnityEngine;

public static class PathValidator
{
    public static bool HasValidPath(CellModel[,] board, CellModel a, CellModel b)
    {
        // --- Trường hợp 1: Cùng hàng, cùng cột, hoặc cùng đường chéo 45° ---
        int dx = b.Row - a.Row;
        int dy = b.Col - a.Col;
        if (IsSameLine(a, b))
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
        return IsNextTo(board, a, b);
    }
    public static bool IsSameLine(CellModel a, CellModel b)
    {
        int dx = b.Row - a.Row;
        int dy = b.Col - a.Col;
        return dx == 0 || dy == 0 || Mathf.Abs(dx) == Mathf.Abs(dy);
    }
    public static bool IsNextTo(CellModel[,] board, CellModel a, CellModel b)
    { // --- Trường hợp 2: Liền kề trên dải 1D row-major, không có cell active nào chắn giữa ---
        int cols = board.GetLength(1);
        int idxA = a.Row * cols + a.Col;
        int idxB = b.Row * cols + b.Col;
        int from = Mathf.Min(idxA, idxB);
        int to = Mathf.Max(idxA, idxB);

        CellModel[] flat = FlattenBoard(board);
        for (int i = from + 1; i < to; i++)
        {
            if (flat[i].IsActive) return false;
        }
        return true;
    }
    public static CellModel[] FlattenBoard(CellModel[,] board)
    {// chuyen board 2d thanh 1d theo thu tu
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        CellModel[] flat = new CellModel[rows * cols];
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                flat[r * cols + c] = board[r, c];
        return flat;
    }
}
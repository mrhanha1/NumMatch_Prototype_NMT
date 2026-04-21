using System.Text;
using UnityEngine;
public static class  AddNumber
{
    public static string FindActivedCells(CellModel[,]board)
    {
        StringBuilder sb = new();
        for (int r=0; r < board.GetLength(0); r++)
            for (int c = 0; c < board.GetLength(1); c++)
            {
                if (board[r, c].IsActive)
                    sb.Append(board[r, c].Value);
                else if (board[r, c].Value == 0)
                {
                    Debug.Log($"All active cells is {sb.ToString()}");
                    return sb.ToString();
                }
            }
        return sb.ToString();
    }
}
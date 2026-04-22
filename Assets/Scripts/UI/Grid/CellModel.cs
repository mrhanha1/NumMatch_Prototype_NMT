public class CellModel
{
    public int Value { get; set; }
    public int Row { get; private set; }
    public int Col { get; private set; }
    public int GemType { get; set; } = 0;

    public bool IsActive = true;
    public bool IsSelected = false;

    public CellModel(int row, int col, int value = 0)
    {
        Row = row;
        Col = col;
        Value = value;
    }
    public void SetPosition(int row, int col)
    {
        Row = row;
        Col = col;
    }
}
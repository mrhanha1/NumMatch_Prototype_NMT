public static class MatchRule
{
    public static bool IsMatch(CellModel a, CellModel b)
    {
        if (a == null || b == null) return false;
        if (!a.IsActive || !b.IsActive) return false;
        if (a.Row == b.Row && a.Col == b.Col) return false;

        int va = a.Value, vb = b.Value;
        return va == vb || va + vb == 10;
    }
}
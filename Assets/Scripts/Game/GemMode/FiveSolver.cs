using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FiveSolver
{
    private const int MAX_DEPTH = 7;
    private const int COLS = 9;
    private static readonly int[] STEPS = { -1, 1, -8, 8, -9, 9, -10, 10 };

    private class Solution
    {
        public List<string> Moves = new List<string>();
        public int TotalCost = 0;
    }

    // Hàm chính
    public static List<string> Solve(List<CellModel> inputList)
    {
        List<Solution> allSolutions = new List<Solution>();
        List<int> fiveList = FindFives(inputList);

        if (fiveList.Count == 0) return new List<string>();

        // Nếu số lượng là lẻ, thử bỏ từng cell
        if (fiveList.Count % 2 == 1)
        {
            for (int skipIndex = 0; skipIndex < fiveList.Count; skipIndex++)
            {
                List<CellModel> clonedList = CloneList(inputList);
                List<int> workingFives = fiveList.Where((_, i) => i != skipIndex).ToList();

                var solutions = SolveWithPerfectMatching(clonedList, workingFives, 0);
                allSolutions.AddRange(solutions);
            }
        }
        else
        {
            List<CellModel> clonedList = CloneList(inputList);
            var solutions = SolveWithPerfectMatching(clonedList, fiveList, 0);
            allSolutions.AddRange(solutions);
        }

        // Sắp xếp và lấy top 10
        return allSolutions
            .OrderBy(s => s.TotalCost)
            .ThenBy(s => s.Moves.Count)
            .Take(10)
            .Select(s => string.Join("|", s.Moves))
            .ToList();
    }

    // Tìm tất cả cell có value == 5
    private static List<int> FindFives(List<CellModel> list)
    {
        List<int> fives = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Value == 5 && list[i].IsActive)
            {
                fives.Add(i);
            }
        }
        return fives;
    }

    // Perfect matching với backtracking
    private static List<Solution> SolveWithPerfectMatching(
        List<CellModel> list,
        List<int> remainingFives,
        int depth)
    {
        List<Solution> solutions = new List<Solution>();

        // Base case: không còn cell nào để ghép
        if (remainingFives.Count == 0)
        {
            solutions.Add(new Solution());
            return solutions;
        }

        // Lấy cell đầu tiên
        int firstIdx = remainingFives[0];
        List<int> rest = remainingFives.Skip(1).ToList();

        // Thử ghép với từng cell còn lại
        for (int i = 0; i < rest.Count; i++)
        {
            int secondIdx = rest[i];

            // Kiểm tra xem có thể match không
            if (!CanMatch(list, firstIdx, secondIdx))
                continue;

            // Clone list để thử match
            List<CellModel> clonedList = CloneList(list);
            List<string> matchMoves = new List<string>();
            int totalCost = 0;

            // Thử match cặp này
            bool success = TryMatchPair(
                clonedList,
                firstIdx,
                secondIdx,
                matchMoves,
                ref totalCost,
                0);

            if (success && totalCost <= MAX_DEPTH)
            {
                // Đánh dấu 2 cell đã match
                clonedList[firstIdx].IsActive = false;
                clonedList[secondIdx].IsActive = false;

                // Tạo move string
                //var (r1, c1) = IndexTo2D(firstIdx);
                //var (r2, c2) = IndexTo2D(secondIdx);
                //matchMoves.Add($"{r1},{c1},{r2},{c2}"); // Move đã được ghi trong TryMatchPair, không cần ghi lại ở đây

                // Đệ quy với các cell còn lại
                List<int> newRemaining = rest.Where((_, idx) => idx != i).ToList();
                var subSolutions = SolveWithPerfectMatching(clonedList, newRemaining, depth + 1);

                // Kết hợp kết quả
                foreach (var subSol in subSolutions)
                {
                    Solution completeSolution = new Solution
                    {
                        Moves = matchMoves.Concat(subSol.Moves).ToList(),
                        TotalCost = totalCost + subSol.TotalCost
                    };
                    solutions.Add(completeSolution);
                }
            }
        }

        return solutions;
    }

    // Thử match một cặp cell, xử lý các blocker bằng đệ quy
    private static bool TryMatchPair(
        List<CellModel> list,
        int idxA,
        int idxB,
        List<string> moves,
        ref int totalCost,
        int currentDepth)
    {
        if (currentDepth > MAX_DEPTH)
            return false;

        // Tìm đường ngắn nhất
        var blockerIndices = FindShortestPath(list, idxA, idxB);

        if (blockerIndices == null)
            return false;

        // Nếu không có blocker, match thành công
        if (blockerIndices.Count == 0)
        {
            totalCost += 0;

            var (r1, c1) = IndexTo2D(idxA);
            var (r2, c2) = IndexTo2D(idxB);
            moves.Add($"{r1},{c1},{r2},{c2}"); //sửa lại

            return true;
        }

        // Có blocker, cần xử lý từng blocker
        List<CellModel> workingList = CloneList(list);
        int subCost = 0;

        foreach (int blockerIdx in blockerIndices)
        {
            if (!workingList[blockerIdx].IsActive)
                continue;

            // Tìm cell để match với blocker này
            var neighbors = FindMatchableNeighbors(workingList, blockerIdx);

            if (neighbors.Count == 0)
                return false;

            // Chọn neighbor có blocker ít nhất
            var bestNeighbor = neighbors.OrderBy(kvp => kvp.Value).First();
            int neighborIdx = bestNeighbor.Key;
            int neighborBlockerCost = bestNeighbor.Value;

            // Đệ quy để match blocker
            List<string> subMoves = new List<string>();
            int recursiveCost = 0;

            bool matchSuccess = TryMatchPair(
                workingList,
                blockerIdx,
                neighborIdx,
                subMoves,
                ref recursiveCost,
                currentDepth + 1);

            if (!matchSuccess)
                return false;

            // Match thành công, đánh dấu inactive
            workingList[blockerIdx].IsActive = false;
            workingList[neighborIdx].IsActive = false;

            //var (r1, c1) = IndexTo2D(blockerIdx);
            //var (r2, c2) = IndexTo2D(neighborIdx);
            //moves.Add($"{r1},{c1},{r2},{c2}");
            moves.AddRange(subMoves);

            subCost += recursiveCost + 1;
        }
        var (ra, ca) = IndexTo2D(idxA);
        var (rb, cb) = IndexTo2D(idxB);
        moves.Add($"{ra},{ca},{rb},{cb}");

        totalCost += subCost;

        // Cập nhật list gốc
        for (int i = 0; i < workingList.Count; i++)
        {
            list[i].IsActive = workingList[i].IsActive;
        }

        return true;
    }

    // Tìm các cell lân cận có thể match
    private static Dictionary<int, int> FindMatchableNeighbors(List<CellModel> list, int cellIdx)
    {
        Dictionary<int, int> neighbors = new Dictionary<int, int>();

        for (int step = 1; step <= 9; step++)
        {
            foreach (int offset in STEPS)
            {
                int targetIdx = cellIdx + offset * step;

                // Kiểm tra điều kiện ngoại lệ
                if (!IsValidStep(cellIdx, offset))
                    continue;

                if (targetIdx < 0 || targetIdx >= list.Count)
                    continue;

                CellModel target = list[targetIdx];

                if (target.IsActive)
                {
                    if (MatchRule.IsMatch(list[cellIdx], target))
                    {
                        // Tính blocker
                        int blockerCount = CountBlockersBetween(list, cellIdx, targetIdx, offset);

                        if (!neighbors.ContainsKey(targetIdx) || neighbors[targetIdx] > blockerCount)
                        {
                            neighbors[targetIdx] = blockerCount;
                        }
                    }
                }
            }
        }

        return neighbors;
    }

    // Tìm đường ngắn nhất giữa 2 cell
    private static List<int> FindShortestPath(List<CellModel> list, int idxA, int idxB)
    {
        int offset = idxB - idxA;
        List<int> bestPath = null;
        int minBlockers = int.MaxValue;

        // Thử các step khác nhau
        foreach (int stepOffset in STEPS)
        {
            if (!IsValidStep(idxA, stepOffset))
                continue;

            if (offset % stepOffset != 0)
                continue;

            int steps = offset / stepOffset;
            if (steps <= 0)
                continue;

            // Đếm blocker trên đường đi này
            List<int> pathBlockers = new List<int>();
            bool validPath = true;

            for (int i = 1; i < steps; i++)
            {
                int checkIdx = idxA + stepOffset * i;

                if (checkIdx < 0 || checkIdx >= list.Count)
                {
                    validPath = false;
                    break;
                }

                if (list[checkIdx].IsActive)
                {
                    pathBlockers.Add(checkIdx);
                }
            }

            if (validPath && pathBlockers.Count < minBlockers)
            {
                minBlockers = pathBlockers.Count;
                bestPath = pathBlockers;
            }
        }

        return bestPath;
    }

    // Đếm số blocker giữa 2 cell
    private static int CountBlockersBetween(List<CellModel> list, int idxA, int idxB, int stepOffset)
    {
        int count = 0;
        int offset = idxB - idxA;

        if (offset % stepOffset != 0)
            return int.MaxValue;

        int steps = offset / stepOffset;

        for (int i = 1; i < Math.Abs(steps); i++)
        {
            int checkIdx = idxA + stepOffset * i;

            if (checkIdx >= 0 && checkIdx < list.Count && list[checkIdx].IsActive)
            {
                count++;
            }
        }

        return count;
    }

    // Kiểm tra điều kiện ngoại lệ
    private static bool IsValidStep(int index, int stepOffset)
    {
        // Cell ở cột cuối (index % 9 == 0)
        if ((index + 1) % COLS == 0)
        {
            if (stepOffset == -10 || stepOffset == 8)
                return false;
        }

        // Cell ở cột đầu ((index + 1) % 9 == 1)
        if (index % COLS == 0)
        {
            if (stepOffset == 10 || stepOffset == -8)
                return false;
        }

        return true;
    }

    // Kiểm tra xem 2 cell có thể match không
    private static bool CanMatch(List<CellModel> list, int idxA, int idxB)
    {
        if (idxA < 0 || idxA >= list.Count || idxB < 0 || idxB >= list.Count)
            return false;

        return MatchRule.IsMatch(list[idxA], list[idxB]);
    }

    // Chuyển đổi index 1D sang 2D
    private static (int row, int col) IndexTo2D(int index)
    {
        int row = index / COLS;
        int col = index % COLS;
        return (row, col);
    }

    // Clone list
    private static List<CellModel> CloneList(List<CellModel> original)
    {
        List<CellModel> cloned = new List<CellModel>();
        foreach (var cell in original)
        {
            var newCell = new CellModel(cell.Row, cell.Col, cell.Value)
            {
                IsActive = cell.IsActive,
                IsSelected = cell.IsSelected,
                GemType = cell.GemType
            };
            cloned.Add(newCell);
        }
        return cloned;
    }
}
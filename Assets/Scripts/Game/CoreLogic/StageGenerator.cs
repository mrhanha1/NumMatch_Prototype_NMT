using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StageGenerator
{

    public static string GenerateStage(int size, int targetPairCount = 2)
    {
        var rng = new System.Random();
        List<int> list = BuildNumberList(size);

        AdjustToTargetPairs(list, targetPairCount);
        Debug.Log($"Generated stage with {FindAllPairs(list).Count} pairs. stage: {string.Concat(list.Select(v => v.ToString()))}");
        return string.Concat(list.Select(v => v.ToString()));
    }


    private static List<(int, int)> FindAllPairs(List<int> list)
    {
        int[] offsets = { 1, 8, 9, 10 };
        int n = list.Count;
        bool[] used = new bool[n];
        var pairs = new List<(int, int)>();

        for (int i = 0; i < n; i++)
        {
            if (used[i] || list[i] == 0) continue;
            foreach (int o in offsets)
            {
                int j = i + o;
                if (j >= n || used[j] || list[j] == 0) continue;
                if (IsMatch(list[i], list[j]))
                {
                    used[i] = used[j] = true;
                    pairs.Add((i, j));
                    break;
                }
            }
        }
        return pairs;
    }
    private static List<int> BuildNumberList(int size)
    {
        var list = new List<int>();
        int baseCount = size / 9;
        int remainder = size % 9;
        for (int i = 1; i <= 9; i++)
            for (int j = 0; j < baseCount; j++)
                list.Add(i);

        var rng = new System.Random();
        var remainList = new List<int>();
        while (remainList.Count < remainder)
        {
            int num = rng.Next(1, 10);
            if (!remainList.Contains(num))
                remainList.Add(num);
        }
        list.AddRange(remainList);
        return list;
    }
    private static bool IsMatch(int a, int b)
    {
        return a == b || a + b == 10;
    }


    public static void AdjustToTargetPairs(List<int> list, int targetPairCount)
    {
        int maxIterations = 1000;
        int iteration = 0;

        while (iteration < maxIterations)
        {
            var currentPairs = FindAllPairs(list);
            int currentCount = currentPairs.Count;

            // Nếu đã đạt target hoặc nhỏ hơn target thì dừng
            if (currentCount <= targetPairCount)
            {
                Debug.Log($"Hoàn thành sau {iteration} lần lặp. Pairs: {currentCount}");
                return;
            }

            // Cần phá vỡ (currentCount - targetPairCount) cặp
            int pairsToBreak = currentCount - targetPairCount;

            // Chọn ngẫu nhiên các cặp cần phá vỡ
            var pairsToDestroy = SelectRandomPairs(currentPairs, pairsToBreak);

            // Phá vỡ các cặp bằng cách swap
            bool success = BreakPairs(list, pairsToDestroy);

            if (!success)
            {
                Debug.LogWarning("Không thể phá vỡ thêm cặp nào");
                break;
            }

            iteration++;
        }

        Debug.Log($"Kết thúc sau {iteration} lần lặp. Pairs cuối: {FindAllPairs(list).Count}");
    }

    /// <summary>
    /// Chọn ngẫu nhiên n cặp từ danh sách
    /// </summary>
    private static List<(int, int)> SelectRandomPairs(List<(int, int)> pairs, int count)
    {
        count = Mathf.Min(count, pairs.Count);
        var shuffled = pairs.OrderBy(x => Random.value).ToList();
        return shuffled.Take(count).ToList();
    }

    /// <summary>
    /// Phá vỡ các cặp đã cho bằng cách swap với vị trí khác
    /// </summary>
    private static bool BreakPairs(List<int> list, List<(int, int)> pairsToBreak)
    {
        bool anySuccess = false;

        foreach (var pair in pairsToBreak)
        {
            // Thử swap một trong hai vị trí của cặp với vị trí khác
            int pos1 = pair.Item1;
            int pos2 = pair.Item2;

            // Thử swap pos1 trước
            if (TryBreakPairBySwap(list, pos1, pos2))
            {
                anySuccess = true;
                continue;
            }

            // Nếu không được, thử swap pos2
            if (TryBreakPairBySwap(list, pos2, pos1))
            {
                anySuccess = true;
            }
        }

        return anySuccess;
    }

    /// <summary>
    /// Thử swap vị trí posToSwap với một vị trí khác để phá vỡ cặp (posToSwap, otherPos)
    /// </summary>
    private static bool TryBreakPairBySwap(List<int> list, int posToSwap, int otherPos)
    {
        int[] offsets = { 1, 8, 9, 10 };

        // Tìm các vị trí có thể swap
        var candidatePositions = GetSwapCandidates(list, posToSwap, otherPos, offsets);

        // Shuffle để tăng tính ngẫu nhiên
        candidatePositions = candidatePositions.OrderBy(x => Random.value).ToList();

        foreach (int swapPos in candidatePositions)
        {
            // Thử swap
            int temp = list[posToSwap];
            list[posToSwap] = list[swapPos];
            list[swapPos] = temp;

            // Kiểm tra xem cặp đã bị phá vỡ chưa
            if (!IsMatch(list[posToSwap], list[otherPos]))
            {
                return true; // Thành công
            }

            // Swap lại nếu không phá vỡ được
            temp = list[posToSwap];
            list[posToSwap] = list[swapPos];
            list[swapPos] = temp;
        }

        return false;
    }

    /// <summary>
    /// Lấy danh sách các vị trí có thể swap với posToSwap
    /// </summary>
    private static List<int> GetSwapCandidates(List<int> list, int posToSwap, int otherPos, int[] offsets)
    {
        var candidates = new List<int>();
        int n = list.Count;

        // Ưu tiên các vị trí không tạo pair mới với các vị trí lân cận
        for (int i = 0; i < n; i++)
        {
            if (i == posToSwap || i == otherPos) continue;
            if (list[i] == 0) continue;

            // Kiểm tra xem swap có tạo pair mới không
            bool createsNewPair = WouldCreateNewPair(list, posToSwap, i, offsets);

            if (!createsNewPair)
            {
                candidates.Add(i);
            }
        }

        // Nếu không có candidate nào, cho phép mọi vị trí
        if (candidates.Count == 0)
        {
            for (int i = 0; i < n; i++)
            {
                if (i != posToSwap && i != otherPos && list[i] != 0)
                {
                    candidates.Add(i);
                }
            }
        }

        return candidates;
    }

    /// <summary>
    /// Kiểm tra xem swap giữa pos1 và pos2 có tạo pair mới không
    /// </summary>
    private static bool WouldCreateNewPair(List<int> list, int pos1, int pos2, int[] offsets)
    {
        int val1 = list[pos1];
        int val2 = list[pos2];

        // Kiểm tra xem val2 tại pos1 có tạo pair với các vị trí lân cận không
        foreach (int offset in offsets)
        {
            int neighborPos = pos1 + offset;
            if (neighborPos >= 0 && neighborPos < list.Count && neighborPos != pos2)
            {
                if (list[neighborPos] != 0 && IsMatch(val2, list[neighborPos]))
                {
                    return true;
                }
            }

            neighborPos = pos1 - offset;
            if (neighborPos >= 0 && neighborPos < list.Count && neighborPos != pos2)
            {
                if (list[neighborPos] != 0 && IsMatch(val2, list[neighborPos]))
                {
                    return true;
                }
            }
        }

        // Kiểm tra xem val1 tại pos2 có tạo pair với các vị trí lân cận không
        foreach (int offset in offsets)
        {
            int neighborPos = pos2 + offset;
            if (neighborPos >= 0 && neighborPos < list.Count && neighborPos != pos1)
            {
                if (list[neighborPos] != 0 && IsMatch(val1, list[neighborPos]))
                {
                    return true;
                }
            }

            neighborPos = pos2 - offset;
            if (neighborPos >= 0 && neighborPos < list.Count && neighborPos != pos1)
            {
                if (list[neighborPos] != 0 && IsMatch(val1, list[neighborPos]))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
using System.Collections.Generic;
using UnityEngine;

public static class GemGenerator
{
    /// <summary>
    /// Generates gem assignments for the given number string.
    /// Only gem types with gemRequired[key] > 0 are candidates.
    /// </summary>
    public static List<int> GenerateGemList(string numberString, Dictionary<int, int> gemRequired)
    {
        var numbers = AddNumber.ParseNumberString(numberString);
        var gemList = new List<int>(new int[numbers.Count]);

        if (gemRequired == null || gemRequired.Count == 0)
            return gemList;

        float X = Random.Range(5f, 7f);
        int Y = Mathf.CeilToInt((numbers.Count + 1) / 2f);

        var availableGemTypes = new List<int>();
        foreach (var kvp in gemRequired)
            if (kvp.Value > 0)
                availableGemTypes.Add(kvp.Key);

        if (availableGemTypes.Count == 0) return gemList;

        int Z = availableGemTypes.Count;
        int gemCountThisRound = 0;
        int counter = 0;
        var selectedGems = new List<(int index, int value, int gemType)>();

        for (int i = 0; i < numbers.Count; i++)
        {
            counter++;
            bool isCandidate = Random.Range(0f, 100f) < X || counter >= Y;

            if (isCandidate && gemCountThisRound < Z)
            {
                int gemType = availableGemTypes[Random.Range(0, availableGemTypes.Count)];

                bool hasMatch = false;
                foreach (var gem in selectedGems)
                {
                    if (numbers[i] == gem.value || numbers[i] + gem.value == 10)
                    {
                        hasMatch = true;
                        break;
                    }
                }

                if (!hasMatch)
                {
                    gemList[i] = gemType;
                    selectedGems.Add((i, numbers[i], gemType));
                    counter = 0;
                    gemCountThisRound++;
                }
            }
        }

        return gemList;
    }
}
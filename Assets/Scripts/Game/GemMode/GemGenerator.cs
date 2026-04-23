using System.Collections.Generic;
using UnityEngine;

public static class GemGenerator
{
    public static List<int> GenerateGemList(string numberString, Dictionary<int, int> gemRequired, Dictionary<int, int> gemCollected)
    {
        var numbers = AddNumber.ParseNumberString(numberString);
        var gemList = new List<int>(new int[numbers.Count]);

        float X = Random.Range(5f, 7f);
        int Y = Mathf.CeilToInt((numbers.Count + 1) / 2f);

        var availableGemTypes = new List<int>();
        foreach (var kvp in gemRequired)
            if (gemCollected[kvp.Key] < kvp.Value)
                availableGemTypes.Add(kvp.Key);

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
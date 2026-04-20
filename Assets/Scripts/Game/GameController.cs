using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting.FullSerializer;
using VContainer;

public class GameController
{
    private readonly GameSession _session;
    private readonly GridView _gridView;

    [Inject]
    public GameController(GameSession session, GridView gridView)
    {
        _session = session;
        _gridView = gridView;
    }

    public void StartGame()
    {
        string input = GenerateStage(32);
        _session.Reset(input);
        _gridView.BuildGrid(_session.Board);
    }

    private string GenerateStage(int size)
    {
        List<int> list = new();
        int baseCount = size / 9;
        int remainder = size % 9;
        for (int i = 1; i <= 9; i++)
        {
            for (int j = 0; j < baseCount; j++)
                list.Add(i);
        }
        List<int> remainlist = new();
        while (remainlist.Count < remainder)
        {
            int num = UnityEngine.Random.Range(1, 10);
            if (!remainlist.Contains(num))
                remainlist.Add(num);
        }
        list.AddRange(remainlist);
        ShuffleList(list);
        StringBuilder sb = new();
        foreach (var num in list) sb.Append(num);
        return sb.ToString();
    }
    void ShuffleList<T>(List<T> list)
    {
        var rng = new Random();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void Restart() => StartGame();
}
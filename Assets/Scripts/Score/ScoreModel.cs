using System;
using UnityEngine;

public class ScoreModel
{
    private int _score;

    public Action<int> OnScoreChanged;

    public void AddScore(int score)
    {
        _score += score;
        OnScoreChanged.Invoke(_score);
    }
}

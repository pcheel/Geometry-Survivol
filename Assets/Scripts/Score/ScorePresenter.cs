using UnityEngine;

public class ScorePresenter
{
    private readonly ScoreModel _model;
    private readonly ScoreView _view;

    public ScorePresenter(ScoreModel scoreModel, ScoreView scoreView)
    {
        _model = scoreModel;
        _view = scoreView;
        _model.OnScoreChanged += ChangeScoreView;
    }
    public void AddScore(int score)
    {
        _model.AddScore(score);
    }
    public void ChangeScoreView(int score)
    {
        _view.SetNewScore(score.ToString());
    }
}

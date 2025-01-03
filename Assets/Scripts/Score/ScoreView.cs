using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    public void SetNewScore(string score)
    {
        _scoreText.text = "Score:" + score;
    }
}

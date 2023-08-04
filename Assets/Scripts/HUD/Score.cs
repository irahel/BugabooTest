using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;

    private void OnEnable()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
        _scoreText.text = "0";

        PlayerScore.ChangedLengthEvent += ChangeScoreText;
    }

    private void OnDisable()
    {
        PlayerScore.ChangedLengthEvent -= ChangeScoreText;
    }

    private void ChangeScoreText(int score)
    {
        _scoreText.text = score.ToString();
    }
}

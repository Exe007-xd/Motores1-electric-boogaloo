using TMPro;
using UnityEngine;

public class FinalScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI endingText;

    private void Start()
    {
        int score = GameResults.FinalScore;

        scoreText.text = $"Puntaje: {score}";

        if (score >= 100)
            endingText.text = "Final Bueno";
        else if (score >= 50)
            endingText.text = "Final Normal";
        else
            endingText.text = "Final Malo";
    }
}

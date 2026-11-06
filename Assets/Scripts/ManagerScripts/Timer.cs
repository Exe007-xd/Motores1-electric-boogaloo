using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float timeRemaining = 150f;
    [SerializeField] private EndingManager endingManager;

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0f;
            if (endingManager != null)
            {
                endingManager.TriggerEnding();
            }
        }


        
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

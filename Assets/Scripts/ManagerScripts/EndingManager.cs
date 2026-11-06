using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [Header("Escenas de final")]
    [SerializeField] private string badEndingScene;
    [SerializeField] private string normalEndingScene;
    [SerializeField] private string goodEndingScene;

    [Header("Puntajes mínimos")]
    [SerializeField] private int normalEndingPoints = 50;
    [SerializeField] private int goodEndingPoints = 150;

    private bool endingTriggered = false;

    public void TriggerEnding()
    {
        if (endingTriggered) return;

        endingTriggered = true;

        int score = PointManager.Instance.TotalPoints;

        GameResults.FinalScore = score;

        string sceneToLoad;

        Cursor.lockState = CursorLockMode.None;

        if (score >= goodEndingPoints)
        {
            sceneToLoad = goodEndingScene;
        }
        else if (score >= normalEndingPoints)
        {
            sceneToLoad = normalEndingScene;
        }
        else
        {
            sceneToLoad = badEndingScene;
        }

        StartCoroutine(FadeController.Instance.FadeOutAndLoadScene(sceneToLoad));
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingUI : MonoBehaviour
{
    [SerializeField] private string gameScene;
    [SerializeField] private string menuScene = "MainMenu";

    public void Retry()
    {
        Time.timeScale = 1f;

        // Reinicia los puntos si es necesario
        if (PointManager.Instance != null)
        {
            Destroy(PointManager.Instance.gameObject);
        }

        SceneManager.LoadScene(gameScene);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;

        if (PointManager.Instance != null)
        {
            Destroy(PointManager.Instance.gameObject);
        }

        SceneManager.LoadScene(menuScene);
    }
}

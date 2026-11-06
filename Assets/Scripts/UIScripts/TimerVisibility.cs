using UnityEngine;

public class TimerVisibility : MonoBehaviour
{
    [SerializeField] private CanvasGroup timerCanvas;
    [SerializeField] private GameObject checklist;

    void Update()
    {
        bool visible = !checklist.activeInHierarchy;

        timerCanvas.alpha = visible ? 1f : 0f;
        timerCanvas.interactable = visible;
        timerCanvas.blocksRaycasts = visible;
    }
}

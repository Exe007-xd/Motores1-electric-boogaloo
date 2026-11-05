using System.Collections;
using TMPro;
using UnityEngine;

public class UIMessageManager : MonoBehaviour
{
    public static UIMessageManager Instance;

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float displayTime = 3f;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        Instance = this;
        messageText.gameObject.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        messageText.gameObject.SetActive(false);
    }
}
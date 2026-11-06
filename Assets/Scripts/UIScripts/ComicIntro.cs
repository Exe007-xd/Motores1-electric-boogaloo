using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class ComicIntro : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;

    [Header("UI")]
    [SerializeField] private GameObject comicPanel;
    [SerializeField] private Image comicImage;

    [Header("Viñetas")]
    [SerializeField] private List<Sprite> pages = new();

    [Header("Transición")]
    [SerializeField] private float fadeDuration = 0.5f;

    private int currentPage = 0;
    private bool isTransitioning;
    private bool comicActive = true;

    private void Start()
    {
        if (pages.Count == 0)
            return;

        Time.timeScale = 0f;

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("UI");

        comicPanel.SetActive(true);
        comicImage.sprite = pages[0];

        Color c = comicImage.color;
        c.a = 1f;
        comicImage.color = c;
    }

    public void OnContinue(InputAction.CallbackContext context)
    {
        if (!comicActive)
            return;

        if (context.performed)
            NextPage();
    }

    public void NextPage()
    {
        if (isTransitioning)
            return;

        if (currentPage + 1 >= pages.Count)
        {
            StartGame();
            return;
        }

        StartCoroutine(ChangePage());
    }

    private IEnumerator ChangePage()
    {
        isTransitioning = true;

        yield return FadeImage(1f, 0f);

        currentPage++;
        comicImage.sprite = pages[currentPage];

        yield return FadeImage(0f, 1f);

        isTransitioning = false;
    }

    private IEnumerator FadeImage(float from, float to)
    {
        float elapsed = 0f;
        Color c = comicImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            c.a = Mathf.Lerp(from, to, elapsed / fadeDuration);
            comicImage.color = c;

            yield return null;
        }

        c.a = to;
        comicImage.color = c;
    }

    private void StartGame()
    {
        comicActive = false;

        comicPanel.SetActive(false);

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("Player");

        Time.timeScale = 1f;
    }
}

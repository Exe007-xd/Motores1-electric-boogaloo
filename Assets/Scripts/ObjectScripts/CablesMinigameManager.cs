using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CablesMinigameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject uiRoot; // panel con botones (desactivado por defecto)
    [SerializeField] private List<CableButton> buttons = new List<CableButton>(); // asigna los botones en el inspector

    private CableButton firstSelected;
    private int matchedCount;
    private Action onComplete;
    private Action onCancel;
    private GameObject player;
    private CinemachineVirtualCameraBase vcam;
    private bool running = false;

    private CursorLockMode originalCursorLockState;
    private bool originalCursorVisible;

    public void StartMinigame(Action onComplete, Action onCancel, GameObject player, CinemachineVirtualCameraBase vcam = null)
    {
        if (running) return;

        this.onComplete = onComplete;
        this.onCancel = onCancel;
        this.player = player;
        this.vcam = vcam;

        matchedCount = 0;
        firstSelected = null;
        foreach (var b in buttons) b.Initialize(this);

        if (uiRoot != null) uiRoot.SetActive(true);

        originalCursorLockState = Cursor.lockState;
        originalCursorVisible = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetPlayerControls(enabled: false);

        running = true;
    }

    public void SelectButton(CableButton btn)
    {
        if (!running || btn.IsMatched) return;

        if (firstSelected == null)
        {
            firstSelected = btn;
            btn.SetSelected(true);
            return;
        }

        if (firstSelected == btn)
        {
            firstSelected.SetSelected(false);
            firstSelected = null;
            return;
        }

        if (firstSelected.CableType == btn.CableType)
        {
            firstSelected.SetMatched();
            btn.SetMatched();
            matchedCount += 2;
            firstSelected = null;

            if (matchedCount >= buttons.Count)
            {
                FinishMinigame(success: true);
            }
        }
        else
        {
            StartCoroutine(HandleMismatch(firstSelected, btn));
            firstSelected = null;
        }
    }

    private System.Collections.IEnumerator HandleMismatch(CableButton a, CableButton b)
    {
        a.SetSelected(false);
        b.SetSelected(true);
        yield return new WaitForSeconds(0.5f);
        b.SetSelected(false);
    }

    private void FinishMinigame(bool success)
    {
        running = false;
        if (uiRoot != null) uiRoot.SetActive(false);
        SetPlayerControls(enabled: true);
        
        Cursor.lockState = originalCursorLockState;
        Cursor.visible = originalCursorVisible;

        if (success) onComplete?.Invoke();
        else onCancel?.Invoke();
    }

    // Aquí: usa PlayerInput (action maps) si existe; si no, fallback a desactivar componentes
    private void SetPlayerControls(bool enabled)
    {
        if (player == null) return;

        var pi = player.GetComponent<PlayerInput>();
        if (pi != null)
        {
            if (enabled)
            {
                // intenta volver al action map de gameplay (ajusta los nombres a tus mapas)
                if (pi.actions.FindActionMap("Player") != null)
                    pi.SwitchCurrentActionMap("Player");
                else
                    pi.enabled = true; // fallback: habilita todo
            }
            else
            {
                // durante minijuego, preferimos un mapa UI/Minigame si existe
                if (pi.actions.FindActionMap("Minigame") != null)
                    pi.SwitchCurrentActionMap("Minigame");
                else if (pi.actions.FindActionMap("UI") != null)
                    pi.SwitchCurrentActionMap("UI");
                else
                    pi.enabled = false; // si no hay mapas, desactiva entradas
            }

            return;
        }

        // Fallback: desactiva componentes concretos
        var interact = player.GetComponent<Player_Interact>();
        if (interact != null) interact.enabled = enabled;

        var mov = player.GetComponent<Mov_Personaje>();
        if (mov != null) mov.enabled = enabled;
    }

    public void CancelMinigame()
    {
        if (!running) return;
        FinishMinigame(success: false);
    }
}
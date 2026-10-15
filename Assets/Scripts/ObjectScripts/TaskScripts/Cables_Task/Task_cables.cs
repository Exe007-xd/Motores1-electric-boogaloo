using System;
using Unity.Cinemachine;
using UnityEngine;



public class Task_cables : Task_Base, IInteractable
{
    [Header("Minijuego")] 
    [SerializeField] private CablesMinigameManager minigameManager; // referencia al manager (puede ser prefab o objeto en escena, desactivado por defecto)
    [SerializeField] private CinemachineVirtualCameraBase minigameVcam; // opcional: vcam que se activará mientras dure el minijuego
    [SerializeField] private int vcamPriorityBoost = 20;

    private bool minigameRunning = false; 
    private int originalVcamPriority;
    
    public void Interact()
    {
        if (isTaskCompleted || minigameRunning) return;

        if (minigameManager == null)
        {
            Debug.LogWarning("No hay MinigameManager asignado en Task_cables.");
            return;
        }

        // Guardar estado y arrancar minijuego
        minigameRunning = true;
        StartTask();

        if (minigameVcam != null)
        {
            originalVcamPriority = minigameVcam.Priority;
            minigameVcam.Priority = originalVcamPriority + vcamPriorityBoost;
        }

        // Desactiva movilidad/inputs del jugador dentro del manager (se pasa el player root)
        var player = GameObject.FindWithTag("Player");

        minigameManager.StartMinigame(
            onComplete: () =>
            {
                // Minijuego completado correctamente
                OnTaskFinished(); // marca la tarea como completada (método de Task_Base)
                CleanupAfterMinigame(player);
            },
            onCancel: () =>
            {
                // Si el minijuego se cancela o falla (opcional)
                CleanupAfterMinigame(player);
            },
            player: player,
            vcam: minigameVcam
        );
    }

    private void CleanupAfterMinigame(GameObject player)
    {
        // Restaurar vcam prioridad
        if (minigameVcam != null)
        {
            minigameVcam.Priority = originalVcamPriority;
        }

        // Permitir nuevas interacciones
        minigameRunning = false;
    }
}

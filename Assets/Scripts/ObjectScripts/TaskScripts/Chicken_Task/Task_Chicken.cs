using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Task_Chicken : Task_Base
{
    [Header("Items")]
    [SerializeField] private ItemDefinition chickenItem;     // item crudo (que el jugador recoge primero)
    [SerializeField] private ItemDefinition roastedItem;     // item rostizado (resultado final)

    [Header("Spawn / tiempos")]
    [SerializeField] private Transform tableSpawnPoint;  // donde aparece el pollo vivo
    [SerializeField] private Transform roasterSpawnPoint;    // donde aparece el roasted_chicken (worldPrefab)
    [SerializeField] private float unfreezingTime = 15f;
    [SerializeField] private float roastingTime = 9f;


    [SerializeField] private AudioSource audioHorno;
    [SerializeField] private Mov_Personaje playerMovement;
    [SerializeField] private Chicken_Movement chickenMovement;
    [SerializeField] private EndingManager endingManager;

    private enum Stage { NotStarted, HasRaw, RawPlaced, LiveSpawned, LiveDeliveredToRoaster, RoastedReady, Completed }
    private Stage stage = Stage.NotStarted;
    [SerializeField] private CinemachineVirtualCameraBase cinematicVcam;
    [SerializeField] private int vcamPriorityBoost = 20;
    private int originalVcamPriority;

    // referencia a la instancia del pollo vivo en escena (si existe)
    private GameObject spawnedLiveChicken;

    void Start()
    {
        if (tableSpawnPoint == null) tableSpawnPoint = this.transform;
        if (roasterSpawnPoint == null) roasterSpawnPoint = this.transform;
    }



    // Llamar desde el objeto que da el item inicial (giver) tras añadir al inventario
    public void NotifyPlayerPickedRaw()
    {
        if (stage != Stage.NotStarted) return;
        stage = Stage.HasRaw;
        StartTask();
        UIMessageManager.Instance.ShowMessage(
                "Pollo agarrado, descongelalo en la tabla."
            );
    }

    // Llamar desde la estación incubadora cuando el jugador intenta colocar el item crudo
    public void PlaceRawInTable()
    {
        if (stage != Stage.HasRaw)
        {
            UIMessageManager.Instance.ShowMessage(
                "Necesito el pollo listo."
            );
            return;
        }

        if (Inventory_Player.Instance == null)
        {
            Debug.LogWarning("Inventory_Player no encontrado.");
            return;
        }

        if (Inventory_Player.Instance.ConsumeItem(chickenItem))
        {
            stage = Stage.RawPlaced;
            UIMessageManager.Instance.ShowMessage(
                "Espera a que descongele el pollo."
            );
            StartCoroutine(UnfreezeAndSpawnLive());
        }
        else
        {
            UIMessageManager.Instance.ShowMessage(
                "No tengo el pollo en el inventario."
            );
        }
    }

    private IEnumerator UnfreezeAndSpawnLive()
    {
        float remaining = unfreezingTime;
        while (remaining > 0f)
        {
            // opcional: notificar UI
            yield return new WaitForSeconds(1f);
            remaining -= 1f;
        }

        yield return SpawnLiveChickenCinematic();
    }

    private IEnumerator SpawnLiveChickenCinematic()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        // Activar cámara cinematográfica
        if (cinematicVcam != null)
        {
            originalVcamPriority = cinematicVcam.Priority;
            cinematicVcam.Priority += vcamPriorityBoost;

        }

        if (chickenItem == null || chickenItem.worldPrefab == null)
        {
            Debug.LogWarning("No se pudo spawnear pollo vivo: falta worldPrefab en chickenItem.");
            stage = Stage.HasRaw;
            yield break;
        }

        // Spawn del pollo
        spawnedLiveChicken = Instantiate(
            chickenItem.worldPrefab,
            tableSpawnPoint.position,
            Quaternion.identity
        );
        chickenMovement = spawnedLiveChicken.GetComponent<Chicken_Movement>();
        if (chickenMovement != null)
        {
            chickenMovement.enabled = false;
        }

        stage = Stage.LiveSpawned;

        UIMessageManager.Instance.ShowMessage(
                "¡Atrapa al pollo!"
            );

        // Duración de la cinemática
        yield return new WaitForSeconds(3f);
        
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        
        if (chickenMovement != null)
        {
            chickenMovement.enabled = true;
        }
        // Volver a la cámara normal
        if (cinematicVcam != null)
        {
            cinematicVcam.Priority = originalVcamPriority;
        }
    }

    // Llamar desde la estación roaster cuando el jugador coloca el pollo vivo (como item) para rostizar
    public void PlaceLiveInRoaster()
    {
        if (stage != Stage.LiveSpawned && stage != Stage.HasRaw)
        {
            UIMessageManager.Instance.ShowMessage(
                "Necesito descongelar el pollo."
            );
            return;
        }

        if (Inventory_Player.Instance == null)
        {
            Debug.LogWarning("Inventory_Player no encontrado.");
            return;
        }

        // El jugador debe haber atrapado el pollo vivo (tener chickenItem en inventario)
        if (Inventory_Player.Instance.ConsumeItem(chickenItem))
        {
            // si la entidad viva sigue en escena, destruirla para evitar duplicados
            if (spawnedLiveChicken != null)
            {
                Destroy(spawnedLiveChicken);
                spawnedLiveChicken = null;
            }

            stage = Stage.LiveDeliveredToRoaster;
            StartCoroutine(RoastAndSpawnRoasted());
        }
        else
        {
            UIMessageManager.Instance.ShowMessage(
                "Lleva el pollo al horno."
            );
        }
    }

    private IEnumerator RoastAndSpawnRoasted()
    {
        float remaining = roastingTime;
        audioHorno.Play();
        while (remaining > 0f)
        {
            yield return new WaitForSeconds(1f);
            remaining -= 1f;
        }

        // spawn roasted item (debe tener worldPrefab configurado)
        if (roastedItem == null || roastedItem.worldPrefab == null)
        {
            Debug.LogWarning("No se pudo spawnear roasted_chicken: falta worldPrefab en roastedItem.");
            // fallback: marcar tarea incompleta pero no bloquear
            
        }

        Instantiate(roastedItem.worldPrefab, roasterSpawnPoint.position, Quaternion.identity);
        stage = Stage.RoastedReady;
        UIMessageManager.Instance.ShowMessage(
                "Pollo listo, llevalo a la mesa."
            );
    }

    // Llamar desde la estación final cuando el jugador coloca el roasted item para completar
    public void PlaceRoastedOnFinish()
    {
        if (stage != Stage.RoastedReady && stage != Stage.LiveDeliveredToRoaster)
        {
            UIMessageManager.Instance.ShowMessage(
                "Todavia no puedo poner el pollo."
            );
            return;
        }

        if (Inventory_Player.Instance == null)
        {
            Debug.LogWarning("Inventory_Player no encontrado.");
            return;
        }

        if (Inventory_Player.Instance.ConsumeItem(roastedItem))
        {
            OnTaskFinished();
            stage = Stage.Completed;
            endingManager.TriggerEnding();
        }
        else
        {
            UIMessageManager.Instance.ShowMessage(
                "Necesito el pollo listo."
            );
        }
    }
}

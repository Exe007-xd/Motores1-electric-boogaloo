using System.Collections;
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
    [SerializeField] private float roastingTime = 5f;

    private enum Stage { NotStarted, HasRaw, RawPlaced, LiveSpawned, LiveDeliveredToRoaster, RoastedReady, Completed }
    private Stage stage = Stage.NotStarted;

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
        Debug.Log("Task_Chicken: jugador obtuvo el pollo crudo (raw). Llevarlo a la mesa.");
    }

    // Llamar desde la estación incubadora cuando el jugador intenta colocar el item crudo
    public void PlaceRawInTable()
    {
        if (stage != Stage.HasRaw)
        {
            Debug.Log("No puedes colocar el pollo crudo ahora.");
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
            Debug.Log("Pollo crudo colocado en la mesa. Comienza el proceso de descongelación.");
            StartCoroutine(UnfreezeAndSpawnLive());
        }
        else
        {
            Debug.Log("No tienes el pollo crudo en el inventario.");
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

        SpawnLiveChicken();
    }

    private void SpawnLiveChicken()
    {
        if (chickenItem == null || chickenItem.worldPrefab == null)
        {
            Debug.LogWarning("No se pudo spawnear pollo vivo: falta worldPrefab en chickenItem.");
            stage = Stage.HasRaw; // fallback
            return;
        }

        spawnedLiveChicken = Instantiate(chickenItem.worldPrefab, tableSpawnPoint.position, Quaternion.identity);
        // Asegúrate: prefab debe contener un componente tipo Chicken_Entity / Chicken_Movement
        stage = Stage.LiveSpawned;
        Debug.Log("Pollo vivo spawneado. Atrápalo y llévalo al roaster.");
    }

    // Llamar desde la estación roaster cuando el jugador coloca el pollo vivo (como item) para rostizar
    public void PlaceLiveInRoaster()
    {
        if (stage != Stage.LiveSpawned && stage != Stage.HasRaw)
        {
            Debug.Log("No puedes colocar pollo en el roaster ahora.");
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
            Debug.Log("Pollo puesto en roaster. Comienza a rostizar...");
            StartCoroutine(RoastAndSpawnRoasted());
        }
        else
        {
            Debug.Log("Necesitas llevar el pollo vivo para ponerlo en el roaster.");
        }
    }

    private IEnumerator RoastAndSpawnRoasted()
    {
        float remaining = roastingTime;
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
        Debug.Log("Roasted chicken listo: recógelo y llévalo a la mesa final para completar la tarea.");
    }

    // Llamar desde la estación final cuando el jugador coloca el roasted item para completar
    public void PlaceRoastedOnFinish()
    {
        if (stage != Stage.RoastedReady && stage != Stage.LiveDeliveredToRoaster)
        {
            Debug.Log("No puedes entregar aún el roasted chicken.");
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
            Debug.Log("Tarea completada: roasted_chicken entregado.");
        }
        else
        {
            Debug.Log("Necesitas llevar el roasted_chicken para entregarlo.");
        }
    }
}

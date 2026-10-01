using UnityEngine;

/// <summary>
/// Inventario simple: solo 1 item a la vez, basado en ItemDefinition (ScriptableObject).
/// No sustituye el item actual al recoger; devuelve false si ya hay uno.
/// Métodos públicos listos para enlazar desde Unity Events (New Input System).
/// </summary>
public class Inventory_Player : MonoBehaviour
{
    [Tooltip("Item que el jugador lleva actualmente (null = vacío)")]
    [SerializeField] private ItemDefinition currentItem;

    public static Inventory_Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public ItemDefinition CurrentItem => currentItem;
    public bool HasItem => currentItem != null;

    /// <summary>
    /// Intenta recoger un item. Devuelve true si se guardó en el slot (si estaba vacío).
    /// </summary>
    public bool AddItem(ItemDefinition def)
    {
        if (def == null)
        {
            Debug.LogWarning("Inventory_Player.AddItem recibió null");
            return false;
        }

        if (currentItem != null)
        {
            Debug.Log("Ya tienes un item. Tira o usa el actual antes de recoger otro.");
            return false;
        }

        currentItem = def;
        Debug.Log($"Item recogido: {def.displayName}");
        return true;
    }

    /// <summary>
    /// Consume (elimina) el item actual si hay uno.
    /// </summary>
    public bool ConsumeItem()
    {
        if (currentItem == null) return false;
        Debug.Log($"Item consumido: {currentItem.displayName}");
        currentItem = null;
        return true;
    }

    /// <summary>
    /// Consume el item actual solo si coincide con la definición dada.
    /// </summary>
    public bool ConsumeItem(ItemDefinition def)
    {
        if (def == null || currentItem == null) return false;
        if (currentItem == def)
        {
            Debug.Log($"Item consumido: {def.displayName}");
            currentItem = null;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Usa/consume el item si coincide con cualquiera de las definiciones dadas (orden no importante).
    /// </summary>
    public bool UseAnyOf(params ItemDefinition[] possibleDefs)
    {
        if (currentItem == null) return false;
        foreach (var d in possibleDefs)
        {
            if (d != null && currentItem == d)
            {
                currentItem = null;
                Debug.Log($"Item usado: {d.displayName}");
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Tira el item que llevas (instancia prefab si está asignado).
    /// Público para enlazar desde Unity Events.
    /// </summary>
    public void DropCurrent()
    {
        if (currentItem == null)
        {
            Debug.Log("Nada que tirar");
            return;
        }

        var def = currentItem;
        currentItem = null;
        SpawnDropped(def);
        Debug.Log($"Item tirado: {def.displayName}");
    }

    private void SpawnDropped(ItemDefinition def)
    {
        if (def == null) return;
        if (def.worldPrefab != null)
        {
            var spawnPos = transform.position + transform.forward * 1f;
            Instantiate(def.worldPrefab, spawnPos, Quaternion.identity);
        }
    }
}

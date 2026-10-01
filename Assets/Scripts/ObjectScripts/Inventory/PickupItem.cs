using UnityEngine;

/// <summary>
/// Pickup que utiliza exclusivamente ItemDefinition.
/// Al interactuar intenta añadir el item al Inventory_Player; si tiene éxito, se destruye el objeto del mundo.
/// </summary>
public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDefinition itemDefinition;

    public void Interact()
    {
        if (itemDefinition == null)
        {
            Debug.LogWarning("PickupItem: falta ItemDefinition en el inspector.");
            return;
        }

        if (Inventory_Player.Instance == null)
        {
            Debug.LogWarning("No hay Inventory_Player en la escena");
            return;
        }

        bool added = Inventory_Player.Instance.AddItem(itemDefinition);
        if (added)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("No se pudo recoger el item (ya llevas uno o error).");
        }
    }
}

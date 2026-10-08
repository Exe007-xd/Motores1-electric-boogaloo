using UnityEngine;

/// <summary>
/// Objeto que da el item inicial (chicken) y notifica a Task_Chicken.
/// Alternativa: puedes usar `PickupItem`, pero este script llama al Task para avanzar el estado.
/// </summary>
public class Chicken_Giver : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDefinition itemToGive;
    [SerializeField] private Task_Chicken taskToNotify;

    public void Interact()
    {
        if (itemToGive == null)
        {
            Debug.LogWarning("Chicken_Giver: falta itemToGive");
            return;
        }

        if (Inventory_Player.Instance == null)
        {
            Debug.LogWarning("No hay Inventory_Player en la escena");
            return;
        }

        if (Inventory_Player.Instance.AddItem(itemToGive))
        {
            Debug.Log("Pollo crudo recogido desde giver.");
            taskToNotify?.NotifyPlayerPickedRaw();
        }
        else
        {
            Debug.Log("No se pudo recoger el pollo: inventario lleno.");
        }
    }
}

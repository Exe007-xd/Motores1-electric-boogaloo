using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Objeto que da el item inicial (chicken) y notifica a Task_Chicken.
/// Alternativa: puedes usar `PickupItem`, pero este script llama al Task para avanzar el estado.
/// </summary>
public class Chicken_Giver : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDefinition itemToGive;
    [SerializeField] private Task_Chicken taskToNotify;
    [SerializeField] private Outline glowObject;
    private bool isAlreadyGiven = false;
    public void Interact()
    {
        if (isAlreadyGiven)
        {
            UIMessageManager.Instance.ShowMessage(
                "Ya entregue el pollo."
            );
            return;
        }

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
            UIMessageManager.Instance.ShowMessage(
                "Pollo recogido."
            ); ;
            taskToNotify?.NotifyPlayerPickedRaw();
        }
        else
        {
            UIMessageManager.Instance.ShowMessage(
                "Tengo el inventario lleno"
            );
        }
    }

    public void OnFocus()
    {
        if (glowObject != null)
        {
            glowObject.enabled = true;
        }
    }
    public void OnLoseFocus()
    {
       if(glowObject != null)
        {
            glowObject.enabled = false; 
        }
    }
}

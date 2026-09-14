using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] Image image;

    //---------
    //Estado
    //---------

    private string inventoryId;
    private Action<string> removeItemAction;

    public void Initialize(string inventoryId, Item item, Action<string> removeItemAction)
    {
        this.inventoryId = inventoryId;
        this.removeItemAction = removeItemAction;

        image.sprite = item.icon;
        transform.localScale = Vector3.one;
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Llama a la acción que elimina / suelta el item en el inventario
            removeItemAction?.Invoke(inventoryId);

            // Elimina la entrada de la UI (este GameObject)
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class Chicken_Entity : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDefinition ItemDefinition;

    public void Interact() 
    {
        if (ItemDefinition == null)
        {
            Debug.LogWarning("Chicken_Entity: falta ItemDefinition");
        }

        if (Inventory_Player.Instance == null)
        {
            Debug.LogWarning("No hay Inventory_player en la escena");
            return;
        }

        bool added = Inventory_Player.Instance.AddItem(ItemDefinition);
        if (added)
        {
            Destroy(gameObject);
            Debug.Log("Pollo recogido");
        }
        else
        {
            Debug.Log("No se pudo recoger el pollo: inventario lleno");
        }

    }
}

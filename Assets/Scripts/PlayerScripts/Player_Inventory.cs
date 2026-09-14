using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider))]
public class Player_Inventory : MonoBehaviour, IInteractor
{
    [Header("Referencias")]
    [SerializeField] InventoryUI ui;

    [Header("Prefabs")]
    [SerializeField] GameObject droppedItemPrefab;

    [Header("Inventario")]
    [SerializeField] SerializedDictionary<string, Item> inventory = new();

    // Ahora público y parte de IInteractor
    public string AddItem(Item item)
    {
        var inventoryId = Guid.NewGuid().ToString();
        inventory.Add(inventoryId, item);

        if (ui != null)
        {
            ui.AddUIItem(inventoryId, item);
        }

        return inventoryId;
    }

    public void DropItem(string inventoryId)
    {
       var droppedItem = Instantiate(droppedItemPrefab, transform.position + transform.forward, Quaternion.identity).GetComponent<DroppedItem>();
       var item = inventory.GetValueOrDefault(inventoryId);
       droppedItem.Initialize(item);
       inventory.Remove(inventoryId);
    }
}



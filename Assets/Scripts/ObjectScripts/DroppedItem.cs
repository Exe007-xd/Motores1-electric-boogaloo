using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class DroppedItem : MonoBehaviour, IInteractable
{
    [Header("Config")]
    [SerializeField] bool autoStart;

    [SerializeField] float enabledPickupDelay = 3f;

    [Header("State")]
    public Item item;
    public bool pickedUp = false;

    private void Start()
    {
        if (autoStart && item != null)
        {
            Initialize(item);
        }
    }

    public void Initialize(Item item)
    {
        this.item = item;
        var droppedItem = Instantiate(item.prefab, transform);
        droppedItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        StartCoroutine(EnablePickupAfterDelay(enabledPickupDelay));
    }

    private IEnumerator<WaitForSeconds> EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Collider>().enabled = true;
    }

   
    public void Interact(IInteractor interactor)
    {
        if (pickedUp) return;
        var col = GetComponent<Collider>();
        if (col != null && !col.enabled) return;

        pickedUp = true;

        if (interactor != null)
        {
            interactor.AddItem(item);
        }

        Destroy(gameObject);
    }
}


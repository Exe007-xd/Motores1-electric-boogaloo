using UnityEngine;

public class OvenStation : MonoBehaviour, IInteractable
{
    [SerializeField] private Task_Chicken task;

    public void Interact()
    {
        if (task == null) Debug.LogWarning("OvenStation: falta referencia a Task_Chicken");
        else task.PlaceLiveInRoaster();
    }
}

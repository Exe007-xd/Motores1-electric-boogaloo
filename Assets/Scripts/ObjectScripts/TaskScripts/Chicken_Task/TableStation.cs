using UnityEngine;

public class TableStation : MonoBehaviour, IInteractable
{
    [SerializeField] private Task_Chicken task;

    public void Interact()
    {
        if (task == null)
        {
            Debug.LogWarning("TableStation: falta referencia a task_chicken ");
        }
        else task.PlaceRawInTable();
        
    }
}

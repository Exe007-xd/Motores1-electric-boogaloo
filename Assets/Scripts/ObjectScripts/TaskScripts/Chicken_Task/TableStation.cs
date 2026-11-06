using UnityEngine;
using UnityEngine.UI;

public class TableStation : MonoBehaviour, IInteractable
{
    [SerializeField] private Task_Chicken task;
    [SerializeField] private Outline glowObject;
    public void Interact()
    {
        if (task == null)
        {
            Debug.LogWarning("TableStation: falta referencia a task_chicken ");
        }
        else task.PlaceRawInTable();
        
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
        if (glowObject != null)
        {
            glowObject.enabled = false;
        }
    }
}

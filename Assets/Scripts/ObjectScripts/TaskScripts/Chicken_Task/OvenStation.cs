using UnityEngine;
using UnityEngine.UI;

public class OvenStation : MonoBehaviour, IInteractable
{
    [SerializeField] private Task_Chicken task;
    [SerializeField] private Outline glowObject; 
    public void Interact()
    {
        if (task == null) Debug.LogWarning("OvenStation: falta referencia a Task_Chicken");
        else task.PlaceLiveInRoaster();
    }
    public void OnFocus()
    {
        glowObject.enabled = true;
    }
    public void OnLoseFocus()
    {
        glowObject.enabled = false;
    }
}

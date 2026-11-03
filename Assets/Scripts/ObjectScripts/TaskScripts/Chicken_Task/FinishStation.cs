using UnityEngine;
using UnityEngine.UI;

public class FinishStation : MonoBehaviour, IInteractable
{
    [SerializeField] private Task_Chicken task;
    [SerializeField] private Outline glowObject;

    public void Interact()
    {
        if (task == null) Debug.LogWarning("FinishStation: falta referencia a Task_Chicken");
        else task.PlaceRoastedOnFinish();
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

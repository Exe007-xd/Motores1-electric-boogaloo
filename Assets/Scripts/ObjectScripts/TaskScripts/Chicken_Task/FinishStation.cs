using UnityEngine;

public class FinishStation : MonoBehaviour
{
    [SerializeField] private Task_Chicken task;
    
    public void Interact()
    {
        if (task == null) Debug.LogWarning("FinishStation: falta referencia a Task_Chicken");
        else task.PlaceRoastedOnFinish();
    }
}

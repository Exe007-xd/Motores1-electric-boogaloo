using UnityEngine;

public class Task_PaperBall : Task_Base, IInteractable
{
    [SerializeField] private ItemDefinition requiredItem;
    [SerializeField] private int requiredAmount = 3;
    [SerializeField] private string deliverMessage = "Paperball entregada";
    private int _currentAmount = 0;


    public void Interact() 
    {
        if (isTaskCompleted)
        { 
            Debug.Log("Tarea ya completada");
            return;
        }

        if (requiredItem == null) 
        {
            Debug.LogWarning("No se ha asignado un item requerido para esta tarea."); 
            return;
        }

        if (Inventory_Player.Instance == null) 
        {
            Debug.LogWarning("No se ha encontrado la instancia de Inventory_Player.");
            return;
        }

        bool consumed = Inventory_Player.Instance.ConsumeItem(requiredItem);

        if (consumed)
        {
            _currentAmount++;
            Debug.Log($"{deliverMessage} ({_currentAmount}/{requiredAmount})");

            if (_currentAmount == 1)
            {
                StartTask();
            }

            if (_currentAmount >= requiredAmount)
            {
                OnTaskFinished();
            }
        }
        else 
        {
            Debug.Log("No tienes el item requerido para entregar.");
        }
    }



}

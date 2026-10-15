using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Zona de la puerta")]
    [SerializeField] private string zoneId;

    private void OnEnable()
    {
        EventManager.TaskUpdated += OnTaskUpdated;
    }

    private void OnDisable()
    {
        EventManager.TaskUpdated -= OnTaskUpdated;
    }
    private void OpenDoor()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTaskUpdated(string taskId, TaskStatus status)
    {
        if (status != TaskStatus.Completed) return;

        var task = Task_Registry.GetTaskById(taskId);
        if (task == null) return;

        if (task.ZoneId != zoneId) return;
        
        var tasks = Task_Registry.GetAllTasks();
        bool allTasksCompleted = true;
        foreach (var t in tasks)
        {
            if (t == null) continue;
            if (t.ZoneId != zoneId) continue;
            if (!t.IsTaskCompleted)
            {
                allTasksCompleted = false;
                break;
            }
        }

        if (allTasksCompleted)
        {
            OpenDoor();
            EventManager.TaskUpdated -= OnTaskUpdated;
        }
    }
}

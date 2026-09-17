using UnityEngine;

public abstract class Task_Base : MonoBehaviour
{
    [SerializeField] private string taskId;
    [SerializeField] private string taskName;
    [TextArea][SerializeField] private string taskDescription;
    protected bool isTaskCompleted = false;

    public string TaskId => string.IsNullOrEmpty(taskId) ? gameObject.name : taskId;
    public string TaskName => string.IsNullOrEmpty(taskName) ? TaskId : taskName;
    public string TaskDescription => string.IsNullOrEmpty(taskDescription) ? string.Empty : taskDescription;
    private void OnEnable()
    {
        Task_Registry.Register(this);
    }

    private void OnDisable()
    {
        Task_Registry.Unregister(this);
    }

    public void StartTask()
    {
        isTaskCompleted = false;
        Debug.Log("Task started: " + TaskId);
        EventManager.RaiseTaskUpdated(TaskId, TaskStatus.InProgress);
    }

    public void OnTaskFinished()
    {
        isTaskCompleted = true;
        Debug.Log("Task completed: " + TaskId);
        EventManager.RaiseTaskUpdated(TaskId, TaskStatus.Completed);
    }


}

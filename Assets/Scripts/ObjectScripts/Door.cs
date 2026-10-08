using UnityEngine;

public class Door : MonoBehaviour
{
  

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
        GameObject door = this.gameObject;
        door.SetActive(!door.activeSelf);
    }

    private void OnTaskUpdated(string taskId, TaskStatus status)
    {
        if (status == TaskStatus.Completed)
        {
         
            OpenDoor();
        }
    }
}

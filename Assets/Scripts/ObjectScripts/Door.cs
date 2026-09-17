using UnityEngine;

public class Door : MonoBehaviour
{
  


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Checklist : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform contentParent; // contenedor donde instanciar filas (RectTransform en Canvas)
    [SerializeField] private GameObject checklistItemPrefab; // prefab con ChecklistItem
    [SerializeField] private GameObject uiRoot;
    private Dictionary<string, ChecklistItem> items = new Dictionary<string, ChecklistItem>();

    private void OnEnable()
    {
        EventManager.TaskUpdated += OnTaskUpdated;
    }

    private void OnDisable()
    {
        EventManager.TaskUpdated -= OnTaskUpdated;
    }

    private void Start()
    {
        // Obtener tareas registradas en lugar de usar FindObjectsOfType
        var tasks = Task_Registry.GetAllTasks();
        foreach (var t in tasks)
        {
            RegisterTask(t, TaskStatus.ToDo);
        }
    }

    private void RegisterTask(Task_Base task, TaskStatus initialStatus)
    {
        if (task == null) return;
        var taskId = task.TaskId;
        if (items.ContainsKey(taskId)) return;

        if (checklistItemPrefab == null || contentParent == null)
        {
            Debug.LogWarning("Checklist: prefab o contentParent no asignado.");
            return;
        }

        var go = Instantiate(checklistItemPrefab, contentParent, worldPositionStays: false);
        var item = go.GetComponent<ChecklistItem>();
        if (item == null)
        {
            Debug.LogWarning("ChecklistItem prefab no tiene ChecklistItem component.");
            Destroy(go);
            return;
        }

        item.Setup(taskId, task.TaskName, task.TaskDescription, initialStatus);
        items.Add(taskId, item);
    }

    private void OnTaskUpdated(string taskId, TaskStatus status)
    {
        if (!items.ContainsKey(taskId))
        {
            var task = Task_Registry.GetTaskById(taskId);
            if (task != null)
            {
                RegisterTask(task, status);
                return;
            }
           
        }
        if (items.ContainsKey(taskId))
        {
            items[taskId].UpdateStatus(status);
        }
            
    }

    public void ToggleChecklist()
    {
        GameObject target = uiRoot != null ? uiRoot : (contentParent != null ? contentParent.gameObject : gameObject);
        target.SetActive(!target.activeSelf);
    }

    public void OnChecklistToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleChecklist();
        }
    }
}

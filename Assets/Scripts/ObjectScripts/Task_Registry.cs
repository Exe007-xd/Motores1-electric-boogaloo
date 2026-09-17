using System.Collections.Generic;

public static class Task_Registry
{
    private static readonly List<Task_Base> tasks = new();

    public static void Register(Task_Base task)
    {
        if (task == null) return;
        if (!tasks.Contains(task)) tasks.Add(task);
    }

    public static void Unregister(Task_Base task)
    {
        if (task == null) return;
        tasks.Remove(task);
    }

    public static IReadOnlyList<Task_Base> GetAllTasks() => tasks.AsReadOnly();

    public static Task_Base GetTaskById(string taskId)
    {
        if (string.IsNullOrEmpty(taskId)) return null;
        for (int i = 0; i < tasks.Count; i++) 
        {
            if (tasks[i].TaskId == taskId) return tasks[i];
        }
        return null;
    }
}

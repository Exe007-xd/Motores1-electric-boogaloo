using System;
using UnityEngine;

public static class EventManager
{
    public static event Action TaskStarted;
    public static event Action TaskEnded;
    public static event Action TaskCancelled;

    
    public static event Action<string, TaskStatus> TaskUpdated;

    public static void RaiseTaskStarted() => TaskStarted?.Invoke();
    public static void RaiseTaskEnded() => TaskEnded?.Invoke();
    public static void RaiseTaskCancelled() => TaskCancelled?.Invoke();

    public static void RaiseTaskUpdated(string taskId, TaskStatus status) => TaskUpdated?.Invoke(taskId, status);
}

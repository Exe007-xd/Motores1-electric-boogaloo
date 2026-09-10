using System;
using UnityEngine;

public static class EventManager
{
    public static Action onTaskStarted;
    public static Action onTaskEnded;
    public static Action onTaskCancelled;

    public static void OnTaskStarted()
    {
        onTaskStarted?.Invoke();
    }

    public static void OnTaskEnded()
    {
        onTaskEnded?.Invoke();
    }

    public static void OnTaskCancelled()
    {
        onTaskCancelled?.Invoke();
    }

}

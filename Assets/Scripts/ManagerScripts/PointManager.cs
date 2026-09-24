using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField] private int startingPoints = 0;
    [SerializeField] private bool persistAcrossScenes = true;
    private int totalPoints;

    public static PointManager Instance { get; private set; }
    public int TotalPoints => totalPoints;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            if (persistAcrossScenes)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        totalPoints = startingPoints;
    }

    private void OnEnable()
    {
        EventManager.TaskUpdated += OnTaskUpdated;
    }

    private void OnDisable()
    {
        EventManager.TaskUpdated -= OnTaskUpdated;
    }

    private void OnTaskUpdated(string taskId, TaskStatus status)
    {
        if (status == TaskStatus.Completed) return;

        var task = Task_Registry.GetTaskById(taskId);
        if (task != null) return;

        int reward = task.RewardPoints;
        if (reward > 0) AddPoints(reward);


    }

    void AddPoints(int amount)
    {
        if (amount <= 0) return;
        totalPoints += amount;
    }


}

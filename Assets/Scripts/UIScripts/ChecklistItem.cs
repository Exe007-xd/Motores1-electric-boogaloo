using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChecklistItem : MonoBehaviour
{
    [SerializeField] internal TMP_Text nameText;
    [SerializeField] internal TMP_Text descriptionText;
    [SerializeField] internal Image statusDot; // opcional, colorea según estado

    public void Setup(string taskId, string displayName, string description, TaskStatus status)
    {
        if (nameText != null) nameText.text = string.IsNullOrEmpty(displayName) ? taskId : displayName;
        if (descriptionText != null) descriptionText.text = description ?? string.Empty;
        UpdateStatus(status);
    }

    public void UpdateStatus(TaskStatus status)
    {      

        if (statusDot != null)
        {
            switch (status)
            {
                case TaskStatus.ToDo:
                    statusDot.color = Color.gray;
                    break;
                case TaskStatus.InProgress:
                    statusDot.color = Color.yellow;
                    break;
                case TaskStatus.Completed:
                    statusDot.color = Color.green;
                    break;
               
            }
        }
    }
}
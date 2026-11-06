using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChecklistItem : MonoBehaviour
{
    [SerializeField] internal TMP_Text nameText;
    [SerializeField] internal TMP_Text descriptionText;

    public void Setup(string taskId, string displayName, string description, TaskStatus status)
    {
        if (nameText != null) nameText.text = string.IsNullOrEmpty(displayName) ? taskId : displayName;
        if (descriptionText != null) descriptionText.text = description ?? string.Empty;
        UpdateStatus(status);
    }

    public void UpdateStatus(TaskStatus status)
    {
        bool completed = status == TaskStatus.Completed;

        if (nameText != null)
        {
            string text = nameText.text.Replace("<s>", "").Replace("</s>", "");
            nameText.text = completed ? $"<s>{text}</s>" : text;
        }

        if (descriptionText != null)
        {
            string text = descriptionText.text.Replace("<s>", "").Replace("</s>", "");
            descriptionText.text = completed ? $"<s>{text}</s>" : text;
        }
    }
}
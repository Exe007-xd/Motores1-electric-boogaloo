using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CableButton : MonoBehaviour
{
    [Header("Configuración")]
    public string CableType; // por ejemplo "rojo", "azul", etc. (asegúrate de emparejar exactamente)
    public Image iconImage; // referencia opcional para cambiar color/gráfico
    public Color matchedColor = Color.green;
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    // Estado
    public bool IsMatched { get; private set; } = false;

    private Button button;
    private CablesMinigameManager manager;
    private Image background;

    public void Initialize(CablesMinigameManager manager)
    {
        this.manager = manager;
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
        IsMatched = false;
        background = GetComponent<Image>();
        if (background != null) background.color = normalColor;
    }

    private void OnClicked()
    {
        if (IsMatched) return;
        manager?.SelectButton(this);
    }

    public void SetSelected(bool selected)
    {
        if (background != null) background.color = selected ? selectedColor : normalColor;
    }

    public void SetMatched()
    {
        IsMatched = true;
        if (background != null) background.color = matchedColor;
        // Desactivar interacción visualmente
        if (button != null) button.interactable = false;
    }
}
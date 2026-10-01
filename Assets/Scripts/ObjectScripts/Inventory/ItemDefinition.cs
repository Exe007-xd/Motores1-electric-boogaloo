using UnityEngine;

[CreateAssetMenu(fileName = "ItemDefinition", menuName = "Inventory/ItemDefinition")]
public class ItemDefinition : ScriptableObject
{
    [Header("Datos")]
    public string displayName;
    public Sprite icon;
    [Tooltip("Prefab que se instanciará en el mundo al tirar el item")]
    public GameObject worldPrefab;
}

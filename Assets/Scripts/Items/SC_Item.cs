using UnityEngine;

[CreateAssetMenu(fileName = "SC_Item", menuName = "ScriptableObjects/NewItem", order = 1)]
public class SC_Item : ScriptableObject
{

    [Header("Item Settings")]
    public string itemName;
    public GameObject itemPrefab;
}
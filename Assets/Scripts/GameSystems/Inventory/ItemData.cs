using GLTFast.Schema;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;

    public Sprite icon;
    public GameObject prefab;

}

using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu]
public class ItemData : ScriptableObject
{
 public int id;   
 public string itemName;
 [Header("prefabs")]
 public GameObject worldPrefab;
 public GameObject heldPrefab;

}

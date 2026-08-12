using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemData> items = new();
    public Transform hand;
    public GameObject currentHeldPrefab;
    private ItemData equippedItem;

    public float dropForce = 100f;


    // Scripttable Object Way
    public void AddItem(ItemData item)
    {
        items.Add(item);
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);

    }




    private void SetupHeldObject(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

    }
 
  // toggle logic
    public void Equip(int index)
    {
        if (index < 0 || index >= items.Count)
            return;
        Unequip();
        equippedItem = items[index];
       
        currentHeldPrefab = Instantiate(
            equippedItem.heldPrefab,
            hand.position,
            hand.rotation,
            hand
        );
        
        SetupHeldObject(currentHeldPrefab);
    }

    private void Unequip()
    {
        if (currentHeldPrefab == null)
            return;
        Destroy(currentHeldPrefab);
        currentHeldPrefab = null;
    }

    
    public void Drop(Transform hand)
    {
        if (equippedItem == null)
            return;

        Instantiate(
           equippedItem.worldPrefab,
            hand.position + hand.forward * 3f,
            Quaternion.identity
        );

        RemoveItem(equippedItem);

        Unequip();

        equippedItem = null;
    } 



   



}
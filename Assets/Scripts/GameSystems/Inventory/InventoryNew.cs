using System.Runtime.CompilerServices;
using UnityEngine;

public class InventoryNew : MonoBehaviour
{
    public ItemData[] slots = new ItemData[6];
    public GameObject[] heldObjects = new GameObject[6];

    public Camera cam;
    public Transform hand;


    public int selectedSlot = -1;


    public bool AddItem(ItemData item, GameObject objectToStore)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                // Hide whatever is currently in hand
                if (selectedSlot != -1)
                {
                    GameObject oldItem = heldObjects[selectedSlot];

                    if (oldItem != null)
                    {
                        oldItem.SetActive(false);
                    }
                }

                // Store new item
                slots[i] = item;
                heldObjects[i] = objectToStore;

                // Put new item in hand
                objectToStore.transform.SetParent(hand);

                ApplyHoldOffsets(objectToStore);
                objectToStore.SetActive(true);

                // Disable physics
                Rigidbody rb = objectToStore.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    if (!rb.isKinematic)
                    {
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                      
                    }
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                selectedSlot = i;

                return true;
            }
        }

        return false;
    }


    public void SelectSlot(int slot)
    {
        if (slot < 0 || slot >= slots.Length)
            return;

        if (slots[slot] == null || heldObjects[slot] == null)
            return;

        //hiding all items

        HideEveryObject();




        // Get selected object
        GameObject item = heldObjects[slot];

        // Show selected object
        item.SetActive(true);

        // Put selected object in hand
        item.transform.SetParent(hand);

        ApplyHoldOffsets(item);

        // Physics off while holding
        Rigidbody rb = item.GetComponent<Rigidbody>();

        if (rb != null)
        {
            if(!rb.isKinematic) {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
              
            }
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        selectedSlot = slot;
    }

    public void HideEveryObject()
    {
        Debug.Log("Hiding objects");
        for (int i = 0; i < heldObjects.Length; i++)
        {
            if (heldObjects[i] != null)
            {
                heldObjects[i].SetActive(false);
            }
        }
    }

    // attachment point 
    public GameObject HeldObject =>
    selectedSlot >= 0 ? heldObjects[selectedSlot] : null;

    public void Drop()
    {
        Debug.Log("Drop");
        if (selectedSlot == -1)
            return;

        GameObject item = heldObjects[selectedSlot];

        if (item == null)
            return;


        // Allow this object to be picked again
        Pickable pickable = item.GetComponent<Pickable>();

        if (pickable != null)
        {
            pickable.ResetPickup();
        }


        // Remove from hand
         item.transform.SetParent(null);
        item.SetActive(true);
        pickable.isPickedup = false;
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);


        // Turn physics back on
        Rigidbody rb = item.GetComponent<Rigidbody>();

        if (rb != null)
        {
            if (!rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            
            }
            rb.isKinematic = false;
            rb.useGravity = true;




            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            Vector3 targetPoint;

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                targetPoint = hit.point;
            else
                targetPoint = ray.origin + ray.direction * 100f;

            Vector3 throwDirection =
                (targetPoint - item.transform.position).normalized;
            ItemData itemData = slots[selectedSlot];

            rb.linearVelocity = throwDirection * itemData.throwSpeed ;
        }



        // Remove from inventory
        slots[selectedSlot] = null;
        heldObjects[selectedSlot] = null;

        selectedSlot = -1;
    }

    public GameObject RemoveHeldItem(GameObject item)
    {
        for (int i = 0; i < heldObjects.Length; i++)
        {
            if (heldObjects[i] == item)
            {
                // Remove from inventory
                slots[i] = null;
                heldObjects[i] = null;

                // If this was selected item
                if (selectedSlot == i)
                {
                    selectedSlot = -1;
                }

                return item;
            }
        }

        return null;
    }

   // Helper for offset 
    private void ApplyHoldOffsets(GameObject itemObj)
    {
        Pickable pickable = itemObj.GetComponent<Pickable>();

        if (pickable != null)
        {
            itemObj.transform.localPosition = pickable.HoldPositionOffset;
            itemObj.transform.localRotation = Quaternion.Euler(pickable.HoldRotationOffset);
        }
        else
        {
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localRotation = Quaternion.identity;
        }
    }


    public ItemData HeldItem
    {
        get
        {
            if (selectedSlot < 0)
                return null;

            return slots[selectedSlot];
        }
    }

}
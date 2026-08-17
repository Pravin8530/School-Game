//using System.Numerics;
using UnityEngine;

public class InventoryNew : MonoBehaviour
{
    public ItemData[] slots = new ItemData[6];
    public GameObject[] heldObjects = new GameObject[6];

    public Camera cam;
    public Transform hand;

    public float throwForce = 500f;

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
                objectToStore.transform.localPosition = Vector3.zero;
                objectToStore.transform.localRotation = Quaternion.identity;

                objectToStore.SetActive(true);

                // Disable physics
                Rigidbody rb = objectToStore.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
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


        // Hide every object
        for (int i = 0; i < heldObjects.Length; i++)
        {
            if (heldObjects[i] != null)
            {
                heldObjects[i].SetActive(false);
            }
        }


        // Get selected object
        GameObject item = heldObjects[slot];

        // Show selected object
        item.SetActive(true);

        // Put selected object in hand
        item.transform.SetParent(hand);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;


        // Physics off while holding
        Rigidbody rb = item.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        selectedSlot = slot;
    }


    public void Drop()
    {
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


        // Turn physics back on
        Rigidbody rb = item.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;




            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            Vector3 targetPoint;

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                targetPoint = hit.point;
            else
                targetPoint = ray.origin + ray.direction * 100f;

            Vector3 throwDirection =
                (targetPoint - item.transform.position).normalized;

            //rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            rb.linearVelocity = throwDirection * throwForce;
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


}
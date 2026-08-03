using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Pickable : MonoBehaviour, IPickable
{
  //cheaking my git profile 
    private Transform playerHand;
    [SerializeField] private float pickUpSpeed = 20f;
    private Rigidbody rb;
    private Renderer rend;
    private Collider col;

    [SerializeField] bool isPickedup = false;
    private Coroutine pickupRoutine;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }



    //player interact can pass hand ref
    public void Interact(Transform hand)
    {
        if (isPickedup) return;

        playerHand = hand;
        rend.enabled = true;
        col.enabled = true;
        isPickedup = true;
        StartCoroutine(PickupRoutine());  // it does both starts coroutine and saves it in var.

    }

    private IEnumerator PickupRoutine()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        while (Vector3.Distance(transform.position, playerHand.position) > 0.05f)
        {

            transform.position = Vector3.Lerp(transform.position, playerHand.position, pickUpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerHand.rotation, pickUpSpeed * Time.deltaTime);

            yield return null;

        }

        transform.position = playerHand.position;
        transform.rotation = playerHand.rotation;
        transform.SetParent(playerHand);

        WorldItem worldItem = GetComponent<WorldItem>();

        Inventory inventory = playerHand.GetComponentInParent<Inventory>();


        inventory.AddItem(worldItem.itemData);

        inventory.Equip(inventory.items.Count - 1);

        Destroy(gameObject);
    }

    // public void Drop()
    // {
    //     GetComponent<Renderer>().enabled = true;
    //     GetComponent<Collider>().enabled = true;
    //     isPickedup = false;

    //     if (pickupRoutine != null)
    //     {
    //         StopCoroutine(pickupRoutine);
    //     }

    //     transform.SetParent(null);
    //     rb.isKinematic = false;
    //     rb.useGravity = true;

    //     rb.linearVelocity = Vector3.zero;
    //     rb.angularVelocity = Vector3.zero;
    //     rb.AddForce(transform.right * forceSpeed, ForceMode.Impulse);


    // }


}

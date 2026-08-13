using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Pickable : MonoBehaviour, IPickable
{
    //cheaking
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




    public void Interact(Transform hand)
    {
        if (isPickedup) return;

        playerHand = hand;
        rend.enabled = true;
        col.enabled = true;
        isPickedup = true;
        StartCoroutine(PickupRoutine());

    }

   

    private IEnumerator PickupRoutine()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.useGravity = false;

        while (Vector3.Distance(transform.position, playerHand.position) > 0.05f)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                playerHand.position,
                pickUpSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                playerHand.rotation,
                pickUpSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = playerHand.position;
        transform.rotation = playerHand.rotation;

        transform.SetParent(playerHand);

        WorldItem worldItem = GetComponent<WorldItem>();

        InventoryNew inventoryNew =
            playerHand.GetComponentInParent<InventoryNew>();

        inventoryNew.AddItem(worldItem.itemData, gameObject);
    }

   

    public void ResetPickup()
    {
        isPickedup = false;

    }
}

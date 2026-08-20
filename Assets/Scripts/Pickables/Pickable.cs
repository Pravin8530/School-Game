using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Pickable : MonoBehaviour, IPickable
{
    [SerializeField] private Vector3 holdPositionOffset = Vector3.zero;
    [SerializeField] private Vector3 holdRotationOffset = Vector3.zero;

    public Vector3 HoldPositionOffset => holdPositionOffset;
    public Vector3 HoldRotationOffset => holdRotationOffset;
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



    // private IEnumerator PickupRoutine()
    // {
    //     rb.linearVelocity = Vector3.zero;
    //     rb.angularVelocity = Vector3.zero;

    //     rb.isKinematic = true;
    //     rb.useGravity = false;

    //     while (Vector3.Distance(transform.position, playerHand.position) > 0.05f)
    //     {
    //            transform.position = Vector3.Lerp(
    //             transform.position,
    //            /* playerHand.position*/
    //             TargetHoldPosition(),
    //             pickUpSpeed * Time.deltaTime
    //         );

    //          transform.rotation = Quaternion.Slerp(
    //             transform.rotation,
    //             /*playerHand.rotation*/
    //             TargetHoldRotation(),
    //             pickUpSpeed * Time.deltaTime
    //         );

    //         yield return null;
    //     }



    //      transform.SetParent(playerHand);
    //      //transform.position = playerHand.position;
    //     //transform.rotation = playerHand.rotation;
    //     transform.position = TargetHoldPosition();
    //     transform.rotation = TargetHoldRotation();

    //     WorldItem worldItem = GetComponent<WorldItem>();

    //     InventoryNew inventoryNew = playerHand.GetComponentInParent<InventoryNew>();

    //     inventoryNew.AddItem(worldItem.itemData, gameObject);
    // }

    private IEnumerator PickupRoutine()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        // Parent immediately so local coordinates match the hand
        transform.SetParent(playerHand, true);

        float timer = 0f;
        float duration = 0.2f; // Takes exactly 0.2 seconds to snap to hand

        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;
        Quaternion targetRot = Quaternion.Euler(holdRotationOffset);

        // Smoothly lerp using a timed loop (100% guaranteed to finish!)
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            transform.localPosition = Vector3.Lerp(startPos, holdPositionOffset, t);
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        // Lock exactly
        transform.localPosition = holdPositionOffset;
        transform.localRotation = targetRot;

        // NOW add to inventory
        WorldItem worldItem = GetComponent<WorldItem>();
        InventoryNew inventoryNew = playerHand.GetComponentInParent<InventoryNew>();

        if (inventoryNew != null && worldItem != null)
        {
            inventoryNew.AddItem(worldItem.itemData, gameObject);
        }
    }

    // private Vector3 TargetHoldPosition()
    // {
    //     if(playerHand==null)return transform.position;
    //     return playerHand.TransformPoint(holdPositionOffset);

    // }

    // private Quaternion TargetHoldRotation()
    // {
    //     if(playerHand==null)return transform.rotation;
    //     return playerHand.rotation * Quaternion.Euler(holdRotationOffset);

    // }

    public void ResetPickup()
    {
        isPickedup = false;

    }
}

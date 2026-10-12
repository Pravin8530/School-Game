using System.Collections;
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

    [SerializeField] public bool isPickedup = false;
    private Coroutine pickupRoutine;

    PlayerStateManager playerState;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }


    public void Interact(Transform hand)
    { 
        if (isPickedup) return;

        if (PlayerStateManager.Instance.currentState == PlayerStateManager.PlayerState.HasImportantItem)
            return;

        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.HoldingItem);

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

        transform.SetParent(playerHand, true);

        float timer = 0f;
        float duration = 0.2f; 

        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;
        Quaternion targetRot = Quaternion.Euler(holdRotationOffset);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            transform.localPosition = Vector3.Lerp(startPos, holdPositionOffset, t);
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        transform.localPosition = holdPositionOffset;
        transform.localRotation = targetRot;

        // add to inventory
        WorldItem worldItem = GetComponent<WorldItem>();
        InventoryNew inventoryNew = playerHand.GetComponentInParent<InventoryNew>();

        if (inventoryNew != null && worldItem != null)
        {
            inventoryNew.AddItem(worldItem.itemData, gameObject);
        }
    }



    public void ResetPickup()
    {
        isPickedup = false;

    }
}

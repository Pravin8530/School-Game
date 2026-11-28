using UnityEngine;

public class Book : MonoBehaviour, IInteractables
{
    [SerializeField] public float distanceFromCamera = 1.2f;
    public bool IsHolding { get; private set; }
    public bool isHolding => IsHolding;
    public int Priority => 1;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 12f;

    [Header("References")]
    public BookShelf bookShelf;
    public string bookColor;
    public bool IsOnShelf { get; set; }

    private Transform targetSlot;

    public void Interact(InventoryNew inventory)
    {
        IsHolding = !IsHolding;

        if (IsHolding)
        {
            targetSlot = null;
            bookShelf?.OnBookPickedUp(this);
            PlayerStateManager.Instance?.SetState(PlayerStateManager.PlayerState.BookInteract);
            return;
        }

        bool placedOnShelf = bookShelf != null && bookShelf.TryPlaceBook(this, transform.position);

        inventory?.RemoveHeldItem(gameObject);
        transform.SetParent(null);

        Pickable pickable = GetComponent<Pickable>();
        WorldItem worldItem = GetComponent<WorldItem>();
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();

        if (placedOnShelf)
        {
            if (pickable != null) Destroy(pickable);
            if (worldItem != null) Destroy(worldItem);

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
        else
        {
            targetSlot = null;
            if (pickable != null) pickable.ResetPickup();

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }

        if (col != null)
        {
            col.enabled = true;
            col.isTrigger = false;
        }

        PlayerStateManager.Instance?.SetState(PlayerStateManager.PlayerState.Normal);
    }

    public void MoveWithRay(Vector3 targetPosition)
    {
        if (!IsHolding)
            return;

        targetPosition.y = transform.position.y;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        bookShelf?.OnBookHover(transform.position);
    }

    public void SetTargetSlot(Transform slot)
    {
        targetSlot = slot;
    }

    private void Update()
    {
        if (IsHolding || targetSlot == null)
            return;

        transform.position = Vector3.Lerp(
            transform.position,
            targetSlot.position,
            moveSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetSlot.rotation,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetSlot.position) < 0.01f)
        {
            transform.SetPositionAndRotation(targetSlot.position, targetSlot.rotation);
            targetSlot = null;
        }
    }

    public void RemoveFromInventory(InventoryNew inventory)
    {
        inventory?.RemoveHeldItem(gameObject);
        transform.SetParent(null);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
            col.isTrigger = false;
        }
    }
}
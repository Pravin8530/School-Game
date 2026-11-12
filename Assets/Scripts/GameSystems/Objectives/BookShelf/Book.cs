
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


    private Transform targetSlot;

    public bool IsOnShelf { get; set; }


    // public void Interact(InventoryNew inventory)
    // {
    //     IsHolding = !IsHolding;

    //     if (IsHolding)
    //     {

    //         targetSlot = null;
    //         bookShelf.OnBookPickedUp(this);

    //         PlayerStateManager.Instance?.SetState(
    //             PlayerStateManager.PlayerState.BookInteract);
    //     }
    //     else
    //     {
    //         bookShelf.OnBookDropped(this, transform.position);

    //         PlayerStateManager.Instance?.SetState(
    //             PlayerStateManager.PlayerState.Normal);
    //     }
    // }

    // public void Interact(InventoryNew inventory)
    // { 

    //     IsHolding = !IsHolding;

    //     if (IsHolding)
    //     {
    //         targetSlot = null;

    //         bookShelf.OnBookPickedUp(this);

    //         PlayerStateManager.Instance?.SetState(
    //             PlayerStateManager.PlayerState.BookInteract);
    //     }
    //     else
    //     {
    //         bookShelf.OnBookDropped(this, transform.position);

    //         // Remove from inventory
    //         inventory.RemoveHeldItem(gameObject);

    //         // Detach from player's hand
    //         transform.SetParent(null);

    //         // Keep physics disabled while on bookshelf
    //         Rigidbody rb = GetComponent<Rigidbody>();

    //         if (rb != null)
    //         {
    //             rb.isKinematic = true;
    //             rb.useGravity = false;
    //         }

    //         PlayerStateManager.Instance?.SetState(
    //             PlayerStateManager.PlayerState.Normal);
    //     }
    // }

    // public void Interact(InventoryNew inventory)
    // {
    //     IsHolding = !IsHolding;

    //     if (IsHolding)
    //     {
    //         targetSlot = null;

    //         bookShelf.OnBookPickedUp(this);

    //         PlayerStateManager.Instance?.SetState(
    //             PlayerStateManager.PlayerState.BookInteract);
    //     }
    //     else
    //     {
    //         bookShelf.OnBookDropped(this, transform.position);

    //         inventory.RemoveHeldItem(gameObject);

    //         // Detach from player's hand
    //         transform.SetParent(null);

    //         // Reset the Pickable state so it can be picked up again from the shelf!
    //         Pickable pickable = GetComponent<Pickable>();
    //         if (pickable != null)
    //         {
    //             pickable.ResetPickup();
    //         }

    //         // Keep physics disabled while on bookshelf
    //         Rigidbody rb = GetComponent<Rigidbody>();

    //         if (rb != null)
    //         {
    //             rb.isKinematic = true;
    //             rb.useGravity = false;
    //         }

    //         Collider col = GetComponent<Collider>();
    //         if (col != null)
    //         {
    //             col.enabled = true;
    //             col.isTrigger = false;
    //         }

    //         PlayerStateManager.Instance?.SetState(
    //             PlayerStateManager.PlayerState.Normal);
    //     }
    // }
public void Interact(InventoryNew inventory)
{ 
    IsHolding = !IsHolding;

    if (IsHolding)
    {
        targetSlot = null;

        bookShelf.OnBookPickedUp(this);

        PlayerStateManager.Instance?.SetState(
            PlayerStateManager.PlayerState.BookInteract);
    }
    else
    {
        bookShelf.OnBookDropped(this, transform.position);

        // Remove from inventory
        inventory.RemoveHeldItem(gameObject);

        // Detach from player's hand
        transform.SetParent(null);

        // --- DISABLE PICKING UP PERMANENTLY ONCE ON THE BOOKSHELF ---
        Pickable pickable = GetComponent<Pickable>();
        if (pickable != null)
        {
            Destroy(pickable); // Removes the Pickable component so it can never be picked up again
        }

        WorldItem worldItem = GetComponent<WorldItem>();
        if (worldItem != null)
        {
            Destroy(worldItem); // Removes world item data if needed
        }
        // -----------------------------------------------------------

        // Keep physics disabled while on bookshelf
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
            col.isTrigger = false; // Still solid for interaction/raycasts if you want to click/hover it for the puzzle
        }

        PlayerStateManager.Instance?.SetState(
            PlayerStateManager.PlayerState.Normal);
    }
}
    public void MoveWithRay(Vector3 targetPosition)
    {
        if (!IsHolding) return;

        targetPosition.y = transform.position.y;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        bookShelf.OnBookHover(transform.position);
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
            transform.SetPositionAndRotation(
                targetSlot.position,
                targetSlot.rotation
            );

            targetSlot = null;
        }
    }

    public void RemoveFromInventory(InventoryNew inventory)
    {
        inventory.RemoveHeldItem(gameObject);

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

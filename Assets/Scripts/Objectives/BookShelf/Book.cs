using UnityEngine;

public class Book : MonoBehaviour, IInteractables
{
    public int Priority => 1;

    [Header("Settings")]
    public float moveSpeed = 12f;
    public float distanceFromCamera = 1.2f;

    [Header("References")]
    public BookShelf bookShelf;

    public bool isHolding;
    public bool IsHolding => isHolding;

    private Transform targetSlot;

    public void Interact()
    {
        isHolding = !isHolding;

        if (isHolding)
        {
            targetSlot = null; // Detach from target slot
        // gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); // Prevent self-raycasting

            bookShelf.OnBookPickedUp(this);

            if (PlayerStateManager.Instance != null)
                PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.BookInteract);
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");

            bookShelf.OnBookDropped(this, transform.position);

            if (PlayerStateManager.Instance != null)
                PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);
        }
    }

     //this is called in player interact heheh--
    public void MoveWithRay(Vector3 targetPosition)
    {
        targetPosition.y = transform.position.y;

        if (!isHolding) return;

        // Smoothly follow raycast ahead of player camera--
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Send current held position to shelf to update the open gap preview
        if (bookShelf != null)
        {
            bookShelf.OnBookHover(transform.position);
        }
    }

    public void SetTargetSlot(Transform slot)
    {
        targetSlot = slot;
    }

    private void Update()
    {
        // Smoothly glide toward assigned target slot on shelf when not held
        if (targetSlot != null && !isHolding)
        {
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

            if (Vector3.Distance(transform.position, targetSlot.position) < 0.001f)
            {
                transform.position = targetSlot.position;
                transform.rotation = targetSlot.rotation;
                targetSlot = null;
            }
        }
    }
}
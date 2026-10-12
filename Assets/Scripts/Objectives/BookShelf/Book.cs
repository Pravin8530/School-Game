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
   
    public string bookColor;

 
    public void Interact(InventoryNew inventory)
    {
        isHolding = !isHolding;

        if (isHolding)
        {
            targetSlot = null; // Detach from target slot

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

    public void MoveWithRay(Vector3 targetPosition)
    {
        targetPosition.y = transform.position.y;

        if (!isHolding) return;

        //follow camera
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

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
        // for going back
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
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.XR;
//***
// detect if (found) callmethods

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract instance;

    [Header("Reycast Distance")]
    public float interactDistance = 4f;

    [Header("Pickable")]

    public IPickable targetPickable;  // item you,re currently looking at

    [Header("Interactable")]
    private IInteractables currentInteractable = null;

    private Inventory inventory;
    private InventoryNew inventoryNew;

    private WorldItem targetWorldItem; // stores worlditem ur looking at

    public ElementCube heldElement;

   private Book heldBook;

    public void Awake()
    {
        inventoryNew = GetComponentInParent<InventoryNew>();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    void Update()
    {

        DetectTargets();
        HandleInput();

    }


    public void DetectTargets()
    {
        targetWorldItem = null;
        targetPickable = null;
        currentInteractable = null;

        Ray ray = new Ray(transform.position, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(ray, interactDistance);

        int highestPriority = -1;

        foreach (RaycastHit hit in hits)
        {

            DetectInteraction(hit, ref highestPriority);
            DetectPickable(hit);

        }


        // if (heldBook != null)
        // {
        //     Vector3 target = ray.origin + ray.direction * heldBook.distanceFromCamera ;
        //     heldBook.MoveWithRay(target);
        // }


        if (heldBook != null)
        {
            if (heldBook.isHolding)
            {
                Vector3 target = ray.origin +
                                ( ray.direction * heldBook.distanceFromCamera) ;

                heldBook.MoveWithRay(target);
            }
            else
            {
                heldBook = null;
            }
        }

    }

    public void DetectInteraction(RaycastHit hit, ref int highestPriority)
    {
        IInteractables interactable =
                hit.collider.GetComponent<IInteractables>();

        if (interactable != null)
        {
            if (interactable.Priority > highestPriority)
            {
                highestPriority = interactable.Priority;
                currentInteractable = interactable;
            }
        }

    }

    public void DetectPickable(RaycastHit hit)
    {

        IPickable pickable =
               hit.collider.GetComponent<IPickable>();

        if (pickable != null)
        {
            targetPickable = pickable;
        }


        WorldItem worldItem = hit.collider.GetComponent<WorldItem>();
        if (worldItem != null)
        {
            targetWorldItem = worldItem;
        }

    }

    public void HandleInput()
    {


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            inventoryNew.SelectSlot(0);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            inventoryNew.SelectSlot(1);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventoryNew.SelectSlot(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            inventoryNew.SelectSlot(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            inventoryNew.SelectSlot(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            inventoryNew.SelectSlot(5);
        }



        if (Input.GetKeyDown(KeyCode.Q))
        {

            inventoryNew.Drop();
            return;
        }


        if (!Input.GetKeyDown(KeyCode.E))
            return;

        if (TryPickup())
            return;

        TryInteract();


    }


    bool TryPickup()
    {
        if (targetPickable == null)
            return false;
        targetPickable.Interact(inventoryNew.hand);
        return true;
    }



    // void TryInteract()
    // {
    //     currentInteractable?.Interact();
    // }

    // void TryInteract()
    // {
    //     if (currentInteractable == null)
    //         return;

    //     Book book = currentInteractable as Book;

    //     if (book != null)
    //     {
    //         heldBook = book;
    //     }
    //    else if(heldBook==book)
    //    {
    //         heldBook=null;
    //     }

    //     currentInteractable.Interact();
    // }

    void TryInteract()
    {
        // SAFEGUARD 1: If we are already holding a book, pressing E should DROP it
        if (heldBook != null)
        {
            heldBook.Interact(); // Toggles isHolding to false & drops onto shelf
            heldBook = null;
            return;
        }

        if (currentInteractable == null)
            return;

        // SAFEGUARD 2: Only pick up a new book if we aren't holding anything
        Book book = currentInteractable as Book;

        if (book != null)
        {
            heldBook = book;
        }

        currentInteractable.Interact();
    }

}

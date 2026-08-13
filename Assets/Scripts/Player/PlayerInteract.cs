using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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

    public void Awake()
    {
        inventory = GetComponentInParent<Inventory>();
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



    void TryInteract()
    {
        currentInteractable?.Interact();
    }



}

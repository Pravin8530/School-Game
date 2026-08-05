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

    private WorldItem targetWorldItem; // stores worlditem ur looking at

    public Transform hand;

    public void Awake()
    {
        inventory = GetComponentInParent<Inventory>();
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
           // may need to change this 

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log($"Items before Equip: {inventory.items.Count}");
            inventory.Equip(0);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventory.Equip(1);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventory.Drop(transform);
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
        targetPickable.Interact(inventory.hand);
        return true;
    }



    void TryInteract()
    {
        currentInteractable?.Interact();
    }




}

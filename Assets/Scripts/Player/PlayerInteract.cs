using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//***
public class PlayerInteract : MonoBehaviour
{
    
public static PlayerInteract instance; 


    [Header("Reycast Distance")]
    public float interactDistance = 2f;

    [Header("Pickble")]
    public IPickable currentPickedItem;
    public IPickable foundPickable;
 
    [Header("Interactable")]
    private IInteractables CurrentInteractable = null;

    bool isHoldingItem = false;
// inventory
    private Inventory inventory;
    private WorldItem worldItem;

    private WorldItem foundWorldItem;


     public void Awake()
    {
        instance = this;
    }

   void Start()
    { //-------
        inventory= GetComponent<Inventory>();
      //-----
    }
    void Update()
    {

        DetectTargets();
        HandleInput(); 


    }


    public void DropCurrentItem()
    {
        if (currentPickedItem == null)
            return;


        currentPickedItem.Drop();
        foundPickable = null;
        currentPickedItem = null;
    }


    public void DetectTargets()
    {
       // foundPickable = null;  // solved the issue with object bellow pickup issue
        CurrentInteractable= null; // same issue with the interactable
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(ray, interactDistance);


        int highestPriority = -1;

        foreach (RaycastHit hit in hits)
        { 

       //-------------------------- inventory    
            //   WorldItem worldItem =hit.collider.GetComponent<WorldItem>();
            //  if(worldItem !=null){
            //   inventory.AddData(worldItem.itemdata);
            //   }

        //---------------------

            /// this is for intract Item like Keys , Door , Drawer , Closet etc :
            Debug.Log(hit.collider.name);

            IInteractables interactable =
                 hit.collider.GetComponent<IInteractables>();

            if (interactable != null)
            {
                if (interactable.Priority > highestPriority)
                {
                    highestPriority = interactable.Priority;
                    CurrentInteractable = interactable;
                }
            }
            ///Pickabes like Key , Paper , etc :

            IPickable pickable =
             hit.collider.GetComponent<IPickable>();

            if (pickable != null)
            {
                foundPickable = pickable;
      
            }
        
             WorldItem worldItem =hit.collider.GetComponent<WorldItem>();
            if (worldItem != null)
            {
                
                foundWorldItem = worldItem;
            }
          
        }

    }
  
   
  
    // void InventoryAddData()
    // {
    //      inventory.AddItem(foundWorldItem.itemdata);
    // }
   
   void InventoryRemoveItem()
    {
      
    }
    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentPickedItem != null)
            {
                DropCurrentItem();

                isHoldingItem = false;
                return;
            }


            if (foundPickable != null && currentPickedItem == null)
            {
                currentPickedItem = foundPickable;
                foundPickable.Interact();
                //InventoryAddData();
                isHoldingItem = true;
                return;
            }

            CurrentInteractable?.Interact();  // short from of if (CurrentInteractable != null) { CurrentInteractable.Interact();

        }


    }
    

    public void ClearHeldItem()
    { 
        foundPickable = null;
        currentPickedItem = null;
       
    }
    

}

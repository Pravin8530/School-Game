using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("raycast")]
    [SerializeField] private float interactionDistance = 5f;
    private RaycastHit hit;


    [Header("input system thing")]
    private PlayerInputActions input;


    [Header("ref to interface")]
    [SerializeField] private IInteractables current;
    
     Inventory inventory;

    private void Awake()
    {
        inventory= GetComponent<Inventory>();
    }

    private void OnEnable()
    {
        input = new PlayerInputActions();

        input.Interaction.Interact.performed += OnInteract;
        input.Interaction.Drop.performed += OnDrop;
        input.Enable();

    }


    private void OnDisable()
    {
        input.Interaction.Interact.performed -= OnInteract;
        input.Interaction.Drop.performed -= OnDrop;
        input.Disable();
    }

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        current = null;
        

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {

            current = hit.collider.GetComponentInParent<IInteractables>();
        
            //currentObjective = hit.collider.GetComponentInParent<IObjective>();

        }


    }

    // since current only works for things we are actully hitting with ray we needed ref to Pickble as heldItem in order to drop it ;
    
//---------------Drop-------------------------------
    private void OnDrop(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
             // inventory.Drop(gameObject.transform);    
    }

//---------------------Interact------------------------
    private void OnInteract(InputAction.CallbackContext context)
    { 
         Debug.Log("interact");
        if (!context.performed) return;
      
        if (current is Pickable pickable)
        {
            pickable.Interact(inventory.hand);

        }
        else
        {
            current?.Interact();

        }
    }
 

 
   
}

using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class ElementCube : MonoBehaviour, IPickable
{

    public ElementType element;

    [SerializeField]
    private Vector3 holdPosition;
    [SerializeField]
    private Vector3 scale;

    [SerializeField]
    private Vector3 rotation;
 
    public bool onBoard= false;
    private Transform originalParent;
    private Rigidbody rb;
    private Collider col;

    [SerializeField]
    private Vector3 originalScale;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

   
    public void Interact(Transform hand)
    { 
        if(onBoard) return; // Prevent interaction if the cube is already on the board
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.HasImportantItem);

        InventoryNew inventory = hand.GetComponentInParent<InventoryNew>();

        if (inventory != null)
        {
            inventory.HideEveryObject();
        }


        PlayerInteract.instance.heldElement = this;
        originalScale = transform.localScale;
        col.enabled = false;
        rb.useGravity = false;
        transform.SetParent(Camera.main.transform);
        transform.localPosition = holdPosition;
        transform.localScale =  scale;
        transform.localRotation = Quaternion.Euler(rotation);

    }
 
  
   
}

public enum ElementType { H, He, Li, Be, B, C, N, O ,Rn }

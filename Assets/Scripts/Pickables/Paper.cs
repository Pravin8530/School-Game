using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class Paper: MonoBehaviour, IPickable
{

    

    [SerializeField]
    private Vector3 holdPosition;
    
    [SerializeField]
    private Vector3 rotation;

    private Transform originalParent;
    private Rigidbody rb;
    private Collider col;

   

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }


    public void Interact(Transform hand)
    {
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.HasImportantItem);

        InventoryNew inventory = hand.GetComponentInParent<InventoryNew>();

        if (inventory != null)
        {
            inventory.HideEveryObject();
        }


        col.enabled = false;
        rb.useGravity = false;
        transform.SetParent(Camera.main.transform);
        transform.localPosition = holdPosition;
        transform.localRotation = Quaternion.Euler(rotation);

    }

   
    
    
   
}


using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeyPickup : MonoBehaviour ,IInteractables
{
    [Header("Key Settings")]
    public string keyID = "key_01";         
    public float bobHeight = 0.15f;         
    public float bobSpeed = 2f;                
    public float pickupRadius = 1.5f;          
    public int Priority => 10;             

    [Header("Position")]
    private Vector3 _startPos;

    [Header("Bool")]
    private bool _collected = false;

    [Header("Collider")]
    [Tooltip("The sphere collider that acts as the pickup trigger")]
    private SphereCollider _trigger;

    [SerializeField]
    [Tooltip("Assign the door that this key unlocks")]
    private  DoorController TargetedDoor;

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        if (_collected) return;
    }



    public void Interact(InventoryNew inventory)
    {
        if (_collected) return;
        _collected = true;


        KeyUIManager uiManager = FindObjectOfType<KeyUIManager>();
        if (uiManager != null)
        {
            uiManager.OnKeyPickedUp(keyID, transform.position);
        }

        gameObject.SetActive(false);
    }

    public void Drop()
    {
        
    }
}

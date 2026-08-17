using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeyPickup : MonoBehaviour ,IInteractables
{
    [Header("Key Settings")]
    public string keyID = "key_01";          // Unique ID (useful for multiple keys)
    public float bobHeight = 0.15f;          // How high the key bobs
    public float bobSpeed = 2f;              // Speed of bobbing
    public float rotateSpeed = 90f;          // Degrees per second rotation
    public float pickupRadius = 1.5f;         // Pickup Trigger
    public int Priority => 10;               // Interaction Raycast priority

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



    public void Interact()
    {
        if (_collected) return;
        _collected = true;

        TargetedDoor.SetLocked(false);                      /// set door to unlocked when key is picked up

        // Tell the UI manager to play the fly-to-corner animation
        KeyUIManager uiManager = FindObjectOfType<KeyUIManager>();
        if (uiManager != null)
        {
            // Pass the world position so the UI can start the animation from here
            uiManager.OnKeyPickedUp(keyID, transform.position);
        }

        // Hide / destroy the 3D key
        gameObject.SetActive(false);
    }

    public void Drop()
    {
        
    }
}

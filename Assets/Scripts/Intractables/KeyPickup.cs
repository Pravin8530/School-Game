using UnityEngine;

/// <summary>
/// Attach this to the 3D Key GameObject on the ground.
/// The key bobs up/down and rotates to attract attention.
/// When the player enters the trigger, it notifies the KeyUIManager.
/// </summary>
[RequireComponent(typeof(Collider))]
public class KeyPickup : MonoBehaviour ,IInteractables
{
    [Header("Key Settings")]
    public string keyID = "key_01";          // Unique ID (useful for multiple keys)
    public float bobHeight = 0.15f;          // How high the key bobs
    public float bobSpeed = 2f;              // Speed of bobbing
    public float rotateSpeed = 90f;          // Degrees per second rotation

    [Header("Pickup Trigger")]
    public float pickupRadius = 1.5f;        // Radius of the sphere trigger

    private Vector3 _startPos;
    private bool _collected = false;
    private SphereCollider _trigger;


    public int Priority => 10;

    public DoorController TargetedDoor;

    void Awake()
    {
        // Set collider as trigger
        //_trigger = GetComponent<SphereCollider>();
        //if (_trigger == null) _trigger = gameObject.AddComponent<SphereCollider>();
       // _trigger.isTrigger = true;
      //  _trigger.radius = pickupRadius;
    }

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        if (_collected) return;

        //// Bob up and down
        //float newY = _startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        //transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        //// Slowly rotate on Y axis
        //transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
       
        // Optionally: Destroy(gameObject, 0.1f);
    }

    public void Interact()
    {
        if (_collected) return;

        // Only the player can pick up

       

        _collected = true;
        TargetedDoor.isDoorLocked = false;
        TargetedDoor.isMoving = false;
        TargetedDoor.lockedDoorAnimation.enabled = false;
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
}

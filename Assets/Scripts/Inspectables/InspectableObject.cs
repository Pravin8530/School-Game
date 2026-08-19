using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Attach to any GameObject you want the player to inspect.
/// Implements IIntractables so PlayerInteract picks it up automatically.
/// 
/// Setup:
///   1. Add this component to your inspectable object.
///   2. Assign the player's Camera to 'inspectCamera' in the Inspector.
///   3. Tweak inspectDistance, inspectSpeed, and rotateSensitivity as needed.
///   4. Optionally disable your PlayerLook / camera controller script
///      by dragging it into 'playerLookScript'.
/// </summary>
public class InspectableObject : MonoBehaviour, IInteractables
{
    [Header("Interact Priority")]
    [SerializeField] private int priority = 1;
    public int Priority => priority;

    [Header("References")]
    [Tooltip("The player's main camera.")]
    public Camera inspectCamera;


    [Header("Inspect Settings")]
    [Tooltip("How far in front of the camera the object floats.")]
    public float inspectDistance = 1.5f;

    [Tooltip("How fast the object lerps to the inspect position.")]
    public float inspectSpeed = 10f;

    [Tooltip("How fast the object lerps to the inspect position.")]
    public float returnSpeed = 1f;



    [Tooltip("Mouse drag sensitivity for rotating the object.")]
    public float rotateSensitivity = 3f;

    
    
 
    // ── private state ──────────────────────────────────────────
    public bool _inspecting;

    public bool isreturning;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Transform _originalParent;


    private Rigidbody _rb;



    // ── IInteractables ──────────────────────────────────────────
    public void Interact()
    {
        if (_inspecting)
        {
            EndInspect();
        }
        else
        {
            BeginInspect();
        }
    }

    // ── Unity ──────────────────────────────────────────────────
    private void Awake()
    {

        _rb = GetComponent<Rigidbody>();

        // Auto-find camera if not assigned
        if (inspectCamera == null)
            inspectCamera = Camera.main;
    }

    private void Update()
    {
        // if (!_inspecting)
        //     return;

        if (_inspecting)
        {
            Vector3 targetPos = inspectCamera.transform.position + inspectCamera.transform.forward * inspectDistance;//, transform.position, Time.deltaTime * inspectSpeed;

            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * inspectSpeed);

            HandleInspectRotation();
        }
        if (isreturning)
        {
            transform.position = Vector3.Lerp(transform.position, _originalPosition, returnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, _originalRotation, returnSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _originalPosition) < 0.01f &&
                Quaternion.Angle(transform.rotation, _originalRotation) < 0.01f)
            {
                transform.SetParent(null);
                if (_rb != null)
                    _rb.isKinematic = false;

                transform.position = _originalPosition;
                transform.rotation = _originalRotation;

                isreturning = false;

            }
        }

    }

    // ── Helpers ────────────────────────────────────────────────
    private void BeginInspect()
    {

        _inspecting = true;

        // Save original transform
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _originalParent = transform.parent;

        // Detach from any parent so world-space movement is clean
        transform.SetParent(_originalParent);

        // Disable physics while inspecting
        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Inspecting);
        Debug.Log(PlayerStateManager.Instance.currentState);
       
         Cursor.lockState = CursorLockMode.None;
         Cursor.visible = true;

    }

    private void EndInspect()
    {
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);
        _inspecting = false;
        isreturning = true;
      

        // Restore cursor to whatever your game normally uses
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void HandleInspectRotation()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.Rotate(inspectCamera.transform.up, -mouseDelta.x * rotateSensitivity, Space.World);
        transform.Rotate(inspectCamera.transform.right, mouseDelta.y * rotateSensitivity, Space.World);
    }
 
    public void Drop()
    {
        

    }

}
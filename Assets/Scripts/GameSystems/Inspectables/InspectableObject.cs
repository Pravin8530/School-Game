using Unity.VisualScripting;
using UnityEngine;
using Unity.Cinemachine;

public class InspectableObject : MonoBehaviour, IInteractables
{
    [SerializeField] private int priority = 1;
    public int Priority => priority;

  
     public Camera inspectCamera;


    public float inspectDistance = 1.5f;

    public float inspectSpeed = 10f;

    public float returnSpeed = 1f;

    public float rotateSensitivity = 3f;




    public bool _inspecting;

    public bool isreturning;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Transform _originalParent;
    private Rigidbody _rb;

    public float minimumInspectDistance = 0.6f;

    public void Interact(InventoryNew inventory)
    {
        if (PlayerStateManager.Instance.currentState == PlayerStateManager.PlayerState.HoldingItem)
        {

            return;
        }
        ;

        if (_inspecting)
        {
            EndInspect();
        }
        else
        {
            float distance = Vector3.Distance(
                transform.position,
                inspectCamera.transform.position
            );

            if (distance < minimumInspectDistance)
            {
                Debug.Log("Too close to inspect!");
                return;
            }

            BeginInspect();
        }
    }

    //  Unity
    private void Awake()
    {

        _rb = GetComponent<Rigidbody>();
        if (inspectCamera == null)

            inspectCamera = Camera.main;
    }

    private void Update()
    {



        if (_inspecting)
        {
            Vector3 targetPos = inspectCamera.transform.position + inspectCamera.transform.forward * inspectDistance;

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

    //  Helpers 
    private void BeginInspect()
    {

        _inspecting = true;

        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _originalParent = transform.parent;

        transform.SetParent(_originalParent);

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


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void HandleInspectRotation()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.Rotate(inspectCamera.transform.up, -mouseDelta.x * rotateSensitivity, Space.World);
        transform.Rotate(inspectCamera.transform.right, mouseDelta.y * rotateSensitivity, Space.World);
    }



}
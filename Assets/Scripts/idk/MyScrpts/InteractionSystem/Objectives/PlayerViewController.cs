using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerViewController : MonoBehaviour
{
    public static PlayerViewController Instance;

    private Transform cam;

    private Transform target;
    private bool isViewing;

    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactMask;

    private float xRotation;
    private float yaw;

    void Awake()
    {
        Instance = this;
       // cam = PlayerStateManager.Instance.cameraTransform;
    }

    void Update()
    {
        if (!isViewing) return;

        MoveCamera();
        HandleLook();
        HandleInteraction();
    }

    public void StartView(Transform viewTarget)
    {
        target = viewTarget;
        isViewing = true;

        xRotation = 0f;
        yaw = cam.parent.eulerAngles.y;
    }

    public void EndView()
    {
        isViewing = false;
        target = null;
    }

    void MoveCamera()
    {
        cam.position = Vector3.Lerp(cam.position, target.position, moveSpeed * Time.deltaTime);
        cam.rotation = Quaternion.Slerp(cam.rotation, target.rotation, moveSpeed * Time.deltaTime);
    }

    void HandleLook()
    {
        Vector2 mouse = Mouse.current.delta.ReadValue() * mouseSensitivity;

        xRotation -= mouse.y;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        yaw += mouse.x;
        cam.parent.rotation = Quaternion.Euler(0, yaw, 0);
    }

    void HandleInteraction()
    {
        if (!Keyboard.current.eKey.wasPressedThisFrame)
            return;

        Camera camera = cam.GetComponent<Camera>();

        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactMask))
        {
            IInteractable obj = hit.collider.GetComponent<IInteractable>();

            if (obj != null)
                obj.Interact();
        }
    }
}
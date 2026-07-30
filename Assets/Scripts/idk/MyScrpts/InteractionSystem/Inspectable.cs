using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static PlayerStateManager;

public class Inspectable : MonoBehaviour, IInteractable
{

    // This script lets the player inspect an object. When `Interact()` is called, the object's current position and rotation should be saved,
    //  the player state changes to `Inspecting`, and the object smoothly moves toward `inspectPos`.
    //  While inspecting, mouse movement (`Mouse.current.delta`) rotates the object around the camera's up and right axes. When `ExitInteract()` is called, 
    // the object stops being inspected and smoothly returns to its saved position and rotation. Once it is close enough to the original location,
    //  the return process ends and the object is detached from any parent.
    //  If you want E to both pick up and put down the object, make `Interact()` toggle between `Inspect()` and `ExitInteract()`.
    //  The main thing to watch out for is saving `originPos` and `originRot` when inspection starts rather than only in `Awake()`,
    //  otherwise the object always returns to its spawn position.


    public Transform inspectPos;
    bool isInspecting = false;
    bool isReturning;

    Vector3 originPos;
    Quaternion originRot;
    [SerializeField] float rotateSpeed = 0.1f;
    float moveSpeed = 10f;
  
    PlayerState previousState;
    PlayerStateManager playerStateManager;
    private void Awake()
    {
        originPos = transform.position;
        originRot = transform.rotation;
    }

    void Update()
    {
        if (isInspecting)
        {
            HandleInspectRotation();
            transform.position = Vector3.Lerp(transform.position, inspectPos.position, moveSpeed * Time.deltaTime);
        }

        if (isReturning)
        {
            transform.position = Vector3.Lerp(transform.position, originPos, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Slerp(transform.rotation, originRot, Time.deltaTime * 10f);

            float distance = Vector3.Distance(transform.position, originPos);

            if (distance < 0.05f)
            {
                isReturning = false;
                isInspecting = false;
                transform.SetParent(null);

            }


        }


    }

   

    public void Interact()
    {
        if (isInspecting)
        {
            Drop();
            return;
        }

        previousState = PlayerStateManager.Instance.currentState;
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Inspecting);

        Inspect();
    }

    private void Inspect()
    {  
        playerStateManager.SetState(PlayerStateManager.PlayerState.Inspecting);
        isInspecting = true;
        isReturning = false;
        transform.SetParent(null);

    }


    public void Drop()
    {
        Debug.Log("droping shit");
        isInspecting = false;
        isReturning = true;

        PlayerStateManager.Instance.SetState(previousState);
    }

    //# issue happend here 1 ( delta is correct?? how to get mouse axis)

    private void HandleInspectRotation()
    {
           
        Vector2 delta = Mouse.current.delta.ReadValue();

        float mouseX = delta.x;
        float mouseY = delta.y;

        transform.Rotate(Camera.main.transform.up, -mouseX * rotateSpeed, Space.World);
        transform.Rotate(Camera.main.transform.right, mouseY * rotateSpeed, Space.World);

    }

}

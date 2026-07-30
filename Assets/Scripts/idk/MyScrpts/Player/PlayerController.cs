
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{  

   
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    private CharacterController controller;
    [Header("Gravity")]
    private float gravity = -9.81f;
    private float verticalVelocity;

    [Header("inputsystem Thingy")]
    [SerializeField] private PlayerInputActions input;
    [SerializeField] private Vector3 moveInput;
    [SerializeField] private Vector3 lookInput;
    

    [Header("Rotation")]
    [SerializeField] public float mouseSensitivity = 20f;
    [SerializeField] private float xRotation = 0f;
    public Transform cameraPivot;

    // [Header("Crouch")]
    // [SerializeField] private float crouchHeight = 0.9f;
    // [SerializeField] private float standingHeight = 1.35f;

     // [SerializeField] private float crouchSpeed = 5f;
    // [SerializeField] private float standSpeed = 10f;


     // [SerializeField] private float crouchTransformY = 0.5f;
     // [SerializeField] private float standingTransformY = 1.5f;
    
    //private bool isCrouching;

    void Awake()
    {
     
     
        controller = GetComponent<CharacterController>();
        input = new PlayerInputActions();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Input__________________
    private void OnEnable()
    {
     
        input.Enable();

        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;


        input.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        input.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        // input.Player.Crouch.performed += ctx => StartCrouch();
        // input.Player.Crouch.canceled += ctx => StopCrouch();

    }

    

    private void OnDisable()
    {
        input.Disable();
    }


    void Update()
    {
        PlayerStateManager.PlayerState state =
        PlayerStateManager.Instance.currentState;
      
        if (state == PlayerStateManager.PlayerState.Inspecting ||
            state == PlayerStateManager.PlayerState.Hiding)
        {
                Debug.Log(PlayerStateManager.Instance.currentState);
            return;
        }
        
        //ading gravity
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2;
        }
        verticalVelocity += gravity * Time.deltaTime;

        PlayerMovement();
        PlayerRotate();
      //  HandleCrouching();
          
    }

    // Movement-----
    private void PlayerMovement()
    {

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        move.y = verticalVelocity;

        controller.Move(move * moveSpeed * Time.deltaTime);
    }


    // Rotating-----
    private void PlayerRotate()
    {
        
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;  // new way ( can clean but later)
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;  //new way

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    // Crouching-----
    // private void StartCrouch()
    // {
    //     Debug.Log("start crouch");
    //     if (PlayerStateManager.Instance.currentState != PlayerStateManager.PlayerState.Normal)
    //         return;
    //     PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Crouching);
    //     moveSpeed = crouchSpeed;


    // }

    // private void StopCrouch()
    // {
    //     Debug.Log("stop crouch");
    //     if (PlayerStateManager.Instance.currentState != PlayerStateManager.PlayerState.Crouching)
    //         return;
    //     PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);
    //     moveSpeed = standSpeed;
       

    // }

    // private void HandleCrouching()
    // {
    //     if (PlayerStateManager.Instance.currentState == PlayerStateManager.PlayerState.Crouching)
    //     {
    //         controller.height = Mathf.Lerp(controller.height, crouchHeight, 2f * Time.deltaTime);
    //         controller.center = new Vector3(0, controller.height / 2f, 0);

    //         Vector3 camPos = cameraPivot.localPosition;
    //         camPos.y = Mathf.Lerp(camPos.y, crouchTransformY, 8f * Time.deltaTime);//crouchTransformY;
    //         cameraPivot.localPosition = camPos;

    //     }
    //     else if (PlayerStateManager.Instance.currentState == PlayerStateManager.PlayerState.Normal)
    //     {
    //         controller.height = Mathf.Lerp(controller.height, standingHeight, 2f * Time.deltaTime);
    //         controller.center = new Vector3(0, controller.height / 2f, 0);

    //         Vector3 camPos = cameraPivot.localPosition;
    //         camPos.y = Mathf.Lerp(camPos.y, standingTransformY, 8f * Time.deltaTime);//standingTransformY;
    //         cameraPivot.localPosition = camPos;

    //     }
    // }


}


using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class HidingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTarget;

    private CharacterController controller;
    private FirstPersonController firstPersonController;

    private Transform exitPoint;
    private Transform originalCameraParent;

    private Vector3 originalCameraLocalPosition;
    private Quaternion originalCameraLocalRotation;

    private Quaternion originalPlayerRotation;

    private bool isHiding;
    private bool canExit;

    public bool IsHiding => isHiding;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        firstPersonController = GetComponent<FirstPersonController>();

        if (cameraTarget == null && firstPersonController != null)
        {
            cameraTarget = firstPersonController
                .CinemachineCameraTarget.transform;
        }

        if (cameraTarget != null)
        {
            originalCameraParent = cameraTarget.parent;
            originalCameraLocalPosition = cameraTarget.localPosition;
            originalCameraLocalRotation = cameraTarget.localRotation;
        }
    }

    public void EnterHiding(
        HidingSpot spot,
        Transform hidePosition,
        Transform exitPosition,
        Transform cameraPoint)
    {
        if (isHiding ||
            hidePosition == null ||
            exitPosition == null)
            return;

        if (PlayerStateManager.Instance != null &&
            PlayerStateManager.Instance.currentState ==
            PlayerStateManager.PlayerState.Inspecting)
            return;

        exitPoint = exitPosition;
        originalPlayerRotation = transform.rotation;

        isHiding = true;
        canExit = false;

        // Teleport player inside the box.
        controller.enabled = false;

        transform.SetPositionAndRotation(
            hidePosition.position,
            hidePosition.rotation
        );

        // Move camera to the peephole.
        if (cameraTarget != null && cameraPoint != null)
        {
            cameraTarget.SetPositionAndRotation(
                cameraPoint.position,
                cameraPoint.rotation
            );
        }

        controller.enabled = false;

        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.SetState(
                PlayerStateManager.PlayerState.Hiding
            );
        }

        StartCoroutine(EnableExit());
    }

    private IEnumerator EnableExit()
    {
        yield return null;
        canExit = true;
    }

    private void Update()
    {
        if (!isHiding || !canExit)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            ExitHiding();
        }
    }

    public void ExitHiding()
    {
        if (!isHiding || exitPoint == null)
            return;

        // Restore the player outside the box.
        transform.SetPositionAndRotation(
            exitPoint.position,
            exitPoint.rotation
        );

        // Restore the camera to its original position.
        if (cameraTarget != null)
        {
            cameraTarget.SetParent(originalCameraParent, false);

            cameraTarget.localPosition = originalCameraLocalPosition;
            cameraTarget.localRotation = originalCameraLocalRotation;
        }

        controller.enabled = true;

        isHiding = false;
        canExit = false;

        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.SetState(
                PlayerStateManager.PlayerState.Normal
            );
        }
    }
}

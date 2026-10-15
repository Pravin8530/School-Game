using StarterAssets;
using System.Collections;
using UnityEngine;

public class Cardboard : MonoBehaviour, IInteractables
{
    [Header("Player Tracking")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerVisualMesh;

    [Header("Hiding Points")]
    [SerializeField] private Transform insidePoint;
    [SerializeField] private Transform outsidePoint;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    public int Priority => 1;

    private bool isInside;
    private bool busy;

    [Header("Box Settings")]
    public Transform DropPoint;
    private Rigidbody boxRigidbody;

    private Transform originalPlayerParent;
    private Vector3 originalPlayerScale;
    private Quaternion fixedPlayerRotation;

    void Awake()
    {
        boxRigidbody = GetComponent<Rigidbody>();
        if (boxRigidbody != null)
        {
            // Keep box kinematic so it never glitches with physics/gravity while worn
            boxRigidbody.isKinematic = true;
            boxRigidbody.useGravity = false;
        }

        if (player != null)
        {
            originalPlayerScale = player.localScale;
            originalPlayerParent = player.parent;
        }
    }

    public void Interact(InventoryNew inventory)
    {
        if (busy) return;
        StartCoroutine(isInside ? Exit() : Enter());
    }

    IEnumerator Enter()
    {
        busy = true;

        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Hiding);

        TogglePlayerPhysics(false);

        // Snap box to drop point
        transform.position = DropPoint.position;
        transform.rotation = DropPoint.rotation;

        // Move player inside the box
        yield return MoveAndRotatePlayer(insidePoint.position, insidePoint.rotation);

        player.localScale = originalPlayerScale * 0.7f;
        fixedPlayerRotation = player.rotation;

        // Parent player to box so they move together
        player.SetParent(transform);

        isInside = true;
        busy = false;
    }

    IEnumerator Exit()
    {
        busy = true;

        // Unparent player
        player.SetParent(originalPlayerParent);
        player.localScale = originalPlayerScale;

        transform.position = DropPoint.position;
        transform.rotation = DropPoint.rotation;

        if (playerVisualMesh != null) playerVisualMesh.SetActive(true);

        // Move player outside
        yield return MoveAndRotatePlayer(outsidePoint.position, outsidePoint.rotation);

        TogglePlayerPhysics(true);
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);

        isInside = false;
        busy = false;
    }

    void LateUpdate()
    {
        if (isInside && !busy && player != null)
        {
            // Keep player rotation locked relative to the box if needed
            player.rotation = fixedPlayerRotation;
        }
    }

    IEnumerator MoveAndRotatePlayer(Vector3 targetPos, Quaternion targetRot)
    {
        while (Vector3.Distance(player.position, targetPos) > 0.02f ||
               Quaternion.Angle(player.rotation, targetRot) > 0.1f)
        {
            player.position = Vector3.MoveTowards(player.position, targetPos, moveSpeed * Time.deltaTime);
            player.rotation = Quaternion.Slerp(player.rotation, targetRot, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        player.position = targetPos;
        player.rotation = targetRot;
    }

    private void TogglePlayerPhysics(bool state)
    {
        if (player == null) return;

        FirstPersonController customMovementScript = player.GetComponent<FirstPersonController>();
        if (customMovementScript != null) customMovementScript.enabled = state;

        CharacterController playerController = player.GetComponent<CharacterController>();

        if (playerController != null)
        {
            if (!state)
            {
                playerController.Move(Vector3.zero);
            }
            playerController.enabled = state;
        }

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.useGravity = state;
            playerRb.isKinematic = !state;

            if (!state)
            {
                playerRb.linearVelocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
                playerRb.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                playerRb.constraints = RigidbodyConstraints.None;
            }
        }
    }
}
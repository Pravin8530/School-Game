using StarterAssets;
using UnityEngine;

public class RollingChair : MonoBehaviour, IInteractables
{
    [SerializeField] private Transform sitPoint;

    [Header("Chair Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float turnSpeed = 120f;

    private FirstPersonController player;
    private bool isRiding;

    public int Priority => 1;

    public void Interact()
    {
        if (!isRiding)
            Sit();
        else
            GetOff();
    }

    private void Sit()
    {
        player = FindFirstObjectByType<FirstPersonController>();

        if (player == null)
            return;

        isRiding = true;

        // Put player on chair
        player.transform.position = sitPoint.position;

        // Make player follow chair
        player.transform.SetParent(transform);

        // Tell player controller to stop normal movement
        player.SetRidingChair(this);
    }

    private void GetOff()
    {
        isRiding = false;

        player.transform.SetParent(null);

        // Step off to the side
        player.transform.position =
            transform.position + transform.right * 0.8f;

        player.SetRidingChair(null);

        player = null;
    }

    private void Update()
    {
        if (!isRiding)
            return;

        // Use the player's EXISTING input
        Vector2 input = player.GetMoveInput();

        // Forward/back
        transform.position +=
            transform.forward *
            input.y *
            moveSpeed *
            Time.deltaTime;

        // Turning
        transform.Rotate(
            Vector3.up,
            input.x * turnSpeed * Time.deltaTime
        );
    }
}
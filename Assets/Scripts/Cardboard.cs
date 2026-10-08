using System.Collections;
using UnityEngine;

public class Cardboard : MonoBehaviour, IInteractables
{
    [Header("Player Tracking")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerVisualMesh; // Drag your capsule mesh graphics here

    [Header("Hiding Points")]
    [SerializeField] private Transform insidePoint;
    [SerializeField] private Transform outsidePoint;
    [SerializeField] private float moveSpeed = 5f;

    public int Priority => 1;

    private bool isInside;
    private bool busy;

    [Header("Box Settings")]
    public Transform DropPoint;
    private Rigidbody boxRigidbody;

    void Awake()
    {
        boxRigidbody = GetComponent<Rigidbody>();
        if (boxRigidbody != null)
        {
            boxRigidbody.isKinematic = true;
        }
    }

    public void Interact()
    {
        if (busy) return;
        StartCoroutine(isInside ? Exit() : Enter());
    }

    IEnumerator Enter()
    {
        busy = true;

        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Hiding);

        // 1. Reset Box Position up into the air
        transform.position = DropPoint.position;
        transform.rotation = DropPoint.rotation;

        // 2. Disable your custom mouse look/movement scripts here if needed 
        // so the player can't walk away while hiding!

        // 3. Move the ENTIRE player capsule into the box
        yield return MovePlayer(insidePoint.position);

        // 4. HIDE the capsule graphics completely so it doesn't poke out of the box
        if (playerVisualMesh != null) playerVisualMesh.SetActive(false);

        // 5. Drop the box over the player!
        if (boxRigidbody != null) boxRigidbody.isKinematic = false;

        isInside = true;
        busy = false;
    }

    IEnumerator Exit()
    {
        busy = true;

        // 1. Pick the box back up into the ceiling
        if (boxRigidbody != null) boxRigidbody.isKinematic = true;
        transform.position = DropPoint.position;
        transform.rotation = DropPoint.rotation;

        // 2. Bring the capsule graphics back to life
        if (playerVisualMesh != null) playerVisualMesh.SetActive(true);

        // 3. Move the player safely back to the outside position
        yield return MovePlayer(outsidePoint.position);

        // 4. Re-enable your mouse look/movement scripts here!

        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);

        isInside = false;
        busy = false;
    }

    IEnumerator MovePlayer(Vector3 target)
    {
        while (Vector3.Distance(player.position, target) > 0.02f)
        {
            player.position = Vector3.MoveTowards(player.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        player.position = target;
    }
}

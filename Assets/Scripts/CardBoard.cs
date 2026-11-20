
using System.Collections;
using StarterAssets;
using Unity.Mathematics;
using UnityEngine;

public class CardBoard : MonoBehaviour, IInteractables
{
    public int Priority => 1;

    public Transform DropPosition;
    public Transform insidePosition;
    public Transform outsidePosition;
    public GameObject Player;

    private FirstPersonController playerScript;
    private CharacterController controller;

    private bool playerInside;
    private bool transitioning;

    private float originalCameraMultiplier;

    private void Awake()
    {
        playerScript = Player.GetComponent<FirstPersonController>();
        controller = Player.GetComponent<CharacterController>();

        originalCameraMultiplier =
            playerScript.CrouchCameraHeightMultiplier;
    }

    public void Interact(InventoryNew inventory)
    {
        if (transitioning) return;

        transform.position = DropPosition.position;
        StartCoroutine(ToggleCardboard());
    }

    private IEnumerator ToggleCardboard()
    {
        transitioning = true;

        if (!playerInside)
        {
            playerScript.CrouchCameraHeightMultiplier = 0.8f;
            playerScript.SetForcedCrouch(true);

            yield return new WaitForEndOfFrame();

            Teleport(insidePosition.position, insidePosition.transform.rotation);

            PlayerStateManager.Instance.SetState(
                PlayerStateManager.PlayerState.Hiding);

            playerInside = true;
        }
        else
        {
             Player.transform.position = outsidePosition.position;
            playerScript.SetForcedCrouch(false);
            playerScript.CrouchCameraHeightMultiplier =
                originalCameraMultiplier;

            PlayerStateManager.Instance.SetState(
                PlayerStateManager.PlayerState.Normal);

            playerInside = false;
        }

      

        transitioning = false;
    }



    private void Teleport(Vector3 position, quaternion rotation)
    {
        controller.enabled = false;
        Player.transform.position = position;
        Player.transform.rotation = rotation;
        controller.enabled = true;
    }
}

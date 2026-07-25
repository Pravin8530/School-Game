using System.Collections;
using UnityEngine;

public class Hideable : MonoBehaviour, IInteractable
{
    [Header("Hide Positions")]
    public Transform hidePosition;
    public Transform exitPosition;

    [Header("Optional Door")]
    public Transform door;
    public float doorOpenAngle = 45f;

    [Header("Animation")]
    public float animationTime = 0.5f;

    private Transform player;
    private bool isBusy;

    private void Start()
    {
       // player = PlayerStateManager.Instance.PlayerTransform;
    }

    public void Interact()
    {
        if (isBusy) return;

        if (PlayerStateManager.Instance.currentState ==
            PlayerStateManager.PlayerState.Hiding)
        {
            StartCoroutine(ExitHide());
        }
        else
        {
            StartCoroutine(EnterHide());
        }
    }

    public void Drop()
    {
        if (isBusy) return;

        StartCoroutine(ExitHide());
    }

    private IEnumerator EnterHide()
    {
        isBusy = true;

        PlayerStateManager.Instance.SetState(
            PlayerStateManager.PlayerState.Hiding);

        if (door != null)
            yield return StartCoroutine(RotateDoor(0f, doorOpenAngle));

        float timer = 0f;

        Vector3 startPos = player.position;
        Quaternion startRot = player.rotation;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;

            float t = timer / animationTime;

            player.position = Vector3.Lerp(
                startPos,
                hidePosition.position,
                t);

            player.rotation = Quaternion.Slerp(
                startRot,
                hidePosition.rotation,
                t);

            yield return null;
        }

        player.position = hidePosition.position;
        player.rotation = hidePosition.rotation;

        if (door != null)
            yield return StartCoroutine(RotateDoor(doorOpenAngle, 0f));

        isBusy = false;
    }

    private IEnumerator ExitHide()
    {
        isBusy = true;

        if (door != null)
            yield return StartCoroutine(RotateDoor(0f, doorOpenAngle));

        float timer = 0f;

        Vector3 startPos = player.position;
        Quaternion startRot = player.rotation;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;

            float t = timer / animationTime;

            player.position = Vector3.Lerp(
                startPos,
                exitPosition.position,
                t);

            player.rotation = Quaternion.Slerp(
                startRot,
                exitPosition.rotation,
                t);

            yield return null;
        }

        player.position = exitPosition.position;
        player.rotation = exitPosition.rotation;

        if (door != null)
            yield return StartCoroutine(RotateDoor(doorOpenAngle, 0f));

        PlayerStateManager.Instance.SetState(
            PlayerStateManager.PlayerState.Normal);

        isBusy = false;
    }

    private IEnumerator RotateDoor(float fromAngle, float toAngle)
    {
        float timer = 0f;

        Quaternion startRot = Quaternion.Euler(0f, fromAngle, 0f);
        Quaternion endRot = Quaternion.Euler(0f, toAngle, 0f);

        while (timer < animationTime * 0.5f)
        {
            timer += Time.deltaTime;

            float t = timer / (animationTime * 0.5f);

            door.localRotation = Quaternion.Slerp(
                startRot,
                endRot,
                t);

            yield return null;
        }

        door.localRotation = endRot;
    }
}
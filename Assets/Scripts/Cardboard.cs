
using System.Collections;
using UnityEngine;

public class Cardboard : MonoBehaviour, IInteractables
{
    [Header("Player")]
    [SerializeField] Transform player;

    [Header("Cardboard Doors")]
    [SerializeField] Transform leftDoor;
    [SerializeField] Transform rightDoor;

    [Header("Player Positions")]
    [SerializeField] Transform insidePoint;
    [SerializeField] Transform outsidePoint;

    [Header("Door Rotation")]
    [SerializeField] float openAngle = 90f;

    [Header("Movement")]
    [SerializeField] float doorSpeed = 5f;
    [SerializeField] float moveSpeed = 3f;
    public int Priority => 1;

    bool isInside;
    bool busy;

    Quaternion leftClosed;
    Quaternion rightClosed;

    Quaternion leftOpen;
    Quaternion rightOpen;

    void Awake()
    {
        leftClosed = leftDoor.localRotation;
        rightClosed = rightDoor.localRotation;

        // Opposite rotations
        leftOpen =
            leftClosed * Quaternion.Euler(-openAngle, 0f,0f);

        rightOpen =
            rightClosed * Quaternion.Euler(openAngle,0f,0f);
    }

    public  void Interact()
    {
        if (busy)
            return;

        StartCoroutine(
            isInside ? Exit() : Enter()
        );
    }

    IEnumerator Enter()
    {
        busy = true;

        // Open BOTH cardboard flaps
        yield return OpenDoors();

        // Move capsule inside
        yield return MovePlayer(
            insidePoint.position
        );

        isInside = true;

        // Close BOTH flaps
        yield return CloseDoors();

        busy = false;
    }

    IEnumerator Exit()
    {
        busy = true;

        // Open BOTH cardboard flaps
        yield return OpenDoors();

        // Move capsule outside
        yield return MovePlayer(
            outsidePoint.position
        );

        isInside = false;

        // Close BOTH flaps
        yield return CloseDoors();

        busy = false;
    }

    IEnumerator OpenDoors()
    {
        while (
            Quaternion.Angle(leftDoor.localRotation, leftOpen) > 0.5f ||
            Quaternion.Angle(rightDoor.localRotation, rightOpen) > 0.5f
        )
        {
            leftDoor.localRotation = Quaternion.Slerp(
                leftDoor.localRotation,
                leftOpen,
                Time.deltaTime * doorSpeed
            );

            rightDoor.localRotation = Quaternion.Slerp(
                rightDoor.localRotation,
                rightOpen,
                Time.deltaTime * doorSpeed
            );

            yield return null;
        }

        leftDoor.localRotation = leftOpen;
        rightDoor.localRotation = rightOpen;
    }

    IEnumerator CloseDoors()
    {
        while (
            Quaternion.Angle(leftDoor.localRotation, leftClosed) > 0.5f ||
            Quaternion.Angle(rightDoor.localRotation, rightClosed) > 0.5f
        )
        {
            leftDoor.localRotation = Quaternion.Slerp(
                leftDoor.localRotation,
                leftClosed,
                Time.deltaTime * doorSpeed
            );

            rightDoor.localRotation = Quaternion.Slerp(
                rightDoor.localRotation,
                rightClosed,
                Time.deltaTime * doorSpeed
            );

            yield return null;
        }

        leftDoor.localRotation = leftClosed;
        rightDoor.localRotation = rightClosed;
    }

    IEnumerator MovePlayer(Vector3 target)
    {
        while (
            Vector3.Distance(player.position, target) > 0.02f
        )
        {
            player.position = Vector3.MoveTowards(
                player.position,
                target,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        player.position = target;
    }
}


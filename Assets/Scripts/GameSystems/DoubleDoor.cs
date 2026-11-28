 using System.Collections;
using UnityEngine;

public class DoubleDoor : MonoBehaviour, IInteractables
{
    private int priority = 1;
    public int Priority => priority;
    public float openAngle = 90f;

    [Header("Rotating doors")]
    public GameObject leftDoor;
    public GameObject rightDoor;


    [Header("rotations")]
    Quaternion openLeftDoorRot;
    Quaternion closeLeftDoorRot;
    Quaternion openRightDoorRot;
    Quaternion closeRightDoorRot;
    private Coroutine currentRoutine;

    bool isOpen;
    float openSpeed = 5f;
    bool isRotating;

    void Start()
    {

        closeLeftDoorRot = transform.localRotation;
        closeRightDoorRot = transform.localRotation;


        openLeftDoorRot = closeLeftDoorRot * Quaternion.Euler(0f, openAngle, 0f);
        openRightDoorRot = closeRightDoorRot *Quaternion.Euler(0f,-openAngle,0f);

    }

    

    public void Interact(InventoryNew inventory)
    {
        if (isRotating) return;
        isOpen = !isOpen;
        Quaternion leftTargetRot;
        Quaternion rightTargetRot;

        leftTargetRot = isOpen ? openLeftDoorRot : closeLeftDoorRot;
        rightTargetRot = isOpen ? openRightDoorRot : closeRightDoorRot;

        if (currentRoutine != null)
        {

            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(DoorRoutine(leftTargetRot,rightTargetRot));

    }


    public IEnumerator DoorRoutine(Quaternion leftRot, Quaternion rightRot)
    {
        isRotating = true;
        while (Quaternion.Angle(leftDoor.transform.localRotation, leftRot) > 0.1f)
        {

            leftDoor.transform.localRotation = Quaternion.Slerp(leftDoor.transform.localRotation, leftRot, Time.deltaTime * openSpeed);
            rightDoor.transform.localRotation = Quaternion.Slerp(rightDoor.transform.localRotation,rightRot, Time.deltaTime*openSpeed);
            yield return null;

        }

       leftDoor.transform.localRotation = leftRot;
       rightDoor.transform.localRotation = rightRot;
        isRotating = false;
        currentRoutine = null;

    }



}

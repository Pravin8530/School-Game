using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using NUnit.Framework;

public class DoorController : MonoBehaviour, IInteractables
{

    [Header("Door Speed And Angle")]
    public float openAngle = 90f;
    public float speed = 120f;
    public float rattleSpeed = 25f;
    public float rattleAngle = 3f;
    public  float rattleTimer;

    [Header("Bool Value")]
    public bool isOpen;
    public bool isMoving;
    public bool isDoorLocked;
    public bool isRattling;

    [Header("Quaternion")]
    Quaternion closedRot;
    Quaternion openRot;



    [Header("Animator")]
    public Animator lockedDoorAnimation;

    [Header("Raycast Priority")]
    public int Priority => 1;


    [Header("Key Requirement")]
    public string requiredKeyID = "key_01";


    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(0, openAngle, 0) * closedRot;

        if (lockedDoorAnimation != null)
            lockedDoorAnimation.enabled = false;

    }

    void Update()
    {
        Quaternion targetRot;

         if (rattleTimer > 0f)
        {
            rattleTimer -= Time.deltaTime;
            float wiggle = Mathf.Sin(Time.time * rattleSpeed) * rattleAngle;
            transform.rotation = closedRot * Quaternion.Euler(0f, 0f,wiggle);
            return;
        } 


        targetRot = isOpen ? openRot : closedRot;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, speed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRot) < 0.1f)
        {
            transform.rotation = targetRot;
            isMoving = false;
        }

    }



    public void Interact(InventoryNew inventory)
    {
        if (isMoving || rattleTimer > 0) return;

        if (isDoorLocked && !isOpen)
        {
            if (!KeyItem.HasKey(requiredKeyID))
            {
               // StartCoroutine(DoorlockedRoutine());

                // if (lockedDoorAnimation != null)
                //     StartCoroutine(DoorLockAnimation());
 
                rattleTimer = 0.3f;
                Debug.Log("Door locked. Required key: " + requiredKeyID);
                return;
            }

            isDoorLocked = false;
            Debug.Log("Door unlocked!");
        }

        isOpen = !isOpen;
        isMoving = true;
    }

    public IEnumerator DoorLockAnimation()
    {
        lockedDoorAnimation.enabled = true;
        lockedDoorAnimation.Play("DoorLockAnim");
        yield return null;
        float animLength = lockedDoorAnimation.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength);

        lockedDoorAnimation.enabled = false;

    }


    public IEnumerator DoorlockedRoutine()
    {
        isRattling = true;
        Quaternion openRattle = closedRot * Quaternion.Euler(0f, 0f,rattleAngle);
        Quaternion closeRattle = closedRot * Quaternion.Euler(0f, 0f, -rattleAngle);

        yield return RattleDoorRoutine(openRattle);
        yield return RattleDoorRoutine(closeRattle);
        yield return RattleDoorRoutine(openRattle);
        yield return RattleDoorRoutine(closeRattle);

        isRattling = false;
    }

    private IEnumerator RattleDoorRoutine(Quaternion targetRot)
    {

        while (Quaternion.Angle(transform.localRotation, targetRot) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * rattleSpeed);
            yield return null;

        }
        transform.localRotation = targetRot;

    }



    public void SetLocked(bool locked)
    {
        isDoorLocked = locked;
    }

}

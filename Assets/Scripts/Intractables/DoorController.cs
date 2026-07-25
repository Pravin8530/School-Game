using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour,IInteractables
{

    [Header("Door Speed And Angle")]
    public float openAngle = 90f;
    public float speed = 120f;


   [Header("Bool Value")]
   bool isOpen;
   public bool isMoving;
   public bool isDoorLocked;

   [Header("Quaternion")]
   Quaternion closedRot;
   Quaternion openRot;

    [Header("Animator")]
    public Animator lockedDoorAnimation;

    [Header("Raycast Priority")]
    public int Priority => 1;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(0, openAngle, 0) * closedRot;
    }

    void Update()
    {
        Quaternion targetRot;
        if (isOpen)
        {
            targetRot = openRot;
        }
        else
        {
            targetRot = closedRot;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, speed * Time.deltaTime);      // Door Open Code 

        if (Quaternion.Angle(transform.rotation, targetRot) < 0.1f)       // Stop movement when fully reached
        {
            transform.rotation = targetRot;
            isMoving = false;
        }

    }

    public void Interact()           // IInteractables function To  Interact 
    {
         if (isMoving) return;

        isOpen = !isOpen;
        isMoving = true;

        if (isDoorLocked)
        {
            StartCoroutine(DoorLockAnimation());
        }
    }

    public IEnumerator DoorLockAnimation()     // Play This Animation When Door is Loocked
    {
        Debug.Log("anim");
        lockedDoorAnimation.Play("DoorLockAnim");
        isOpen = false;
        isMoving = false;
        yield return null;
        Debug.Log("anim1");
       
    }
    

}

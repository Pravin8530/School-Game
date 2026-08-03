using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour,IInteractables
{

    [Header("Door Speed And Angle")]
    public float openAngle = 90f;
    public float speed = 120f;


   [Header("Bool Value")]
   public bool isOpen;
   public bool isMoving;
   public bool isDoorLocked;

   [Header("Quaternion")]
   Quaternion closedRot;
   Quaternion openRot;

    [Header("Animator")]
    public Animator lockedDoorAnimation;

    [Header("Raycast Priority")]
    public int Priority => 1;


    // IMP TIP - you cant set isOpen And isDoorLocked bool True cuz either  door is locked or door is open ; 

    // if you want to set door locked then set isDoorLocked = true and isOpen = false ;

    // if you want door intial open the set isOpen = true and isDoorLocked = false ;


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
        Debug.Log("Intract Door");

        if (isMoving) return;
        Debug.Log("Intract Door 1 ");
        if (isDoorLocked && !isOpen)
        {
            Debug.Log("Intract Door 2 ");
            StartCoroutine(DoorLockAnimation());
            return;
        }

        isOpen = !isOpen;
        isMoving = true;
        Debug.Log("Intract Door 3");

    }

    public IEnumerator DoorLockAnimation()     // Play This Animation When Door is Loocked
    {

        lockedDoorAnimation.enabled = true;
        lockedDoorAnimation.Play("DoorLockAnim");
        yield return null; 
        float animLength = lockedDoorAnimation.GetCurrentAnimatorStateInfo(0).length;               //  Get the length of the animation clip 
        yield return new WaitForSeconds(animLength);                                        // Wait for the animation to finish

        lockedDoorAnimation.enabled = false;
  

    }
    public void SetLocked(bool locked)                  // Function to set the door's locked state
    {
        isDoorLocked = locked;
    }

}

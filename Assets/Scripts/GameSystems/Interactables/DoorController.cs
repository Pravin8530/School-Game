using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour, IInteractables
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


    [Header("Key Requirement")]
    public string requiredKeyID = "key_01";
   
   //public Quaternion targetRot;

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

    // public void Interact(InventoryNew inventory)           
    // {
    //     Debug.Log("Intract Door");

    //     if (isMoving) return;
    //     Debug.Log("Intract Door 1 ");
    //     if (isDoorLocked && !isOpen)
    //     {
    //         Debug.Log("Intract Door 2 ");
    //         StartCoroutine(DoorLockAnimation());
    //         return;
    //     }

    //     isOpen = !isOpen;
    //     isMoving = true;
    //     Debug.Log("Intract Door 3");

    // }

    
public void Interact(InventoryNew inventory)
{
    if (isMoving) return;

    if (isDoorLocked && !isOpen)
    {
        // Play the locked animation if the player has no key.
        if (!KeyItem.HasKey(requiredKeyID))
        {
            if (lockedDoorAnimation != null)
                StartCoroutine(DoorLockAnimation());

            Debug.Log("Door locked. Required key: " + requiredKeyID);
            return;
        }

        // The correct key was collected.
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
    public void SetLocked(bool locked)                  
    {
        isDoorLocked = locked;
    }

}

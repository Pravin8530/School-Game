using UnityEngine;
using System.Collections;

public class ClosetController : MonoBehaviour, IInteractables
{

    [Header("Camera Ref")]
    public GameObject playerCamera;
    public GameObject closetCamera;

    [Header(" Player Reference")]
    public GameObject player;

    [Header("Bool Value")]
    public bool insideCloset;
    public bool canIntract;

    [Header("Animator Ref")]
    public Animator doorAnim;

    [Header("Reycast Priority")]
    public int Priority => 1;

    [Header("PlayerPosition")]

    public Transform hidingPosition;
    public Transform exitPosition;

    public bool isTransitioning;

   


    public IEnumerator InsideCloset()           // Player Inside Closet
    {  isTransitioning=true;
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Hiding);
        Debug.Log("inside Closet");
         insideCloset = true;
         canIntract=false;
        doorAnim.Play("DoorOpenAndClose");
        yield return new WaitForSeconds(0.5f);

        float duration = 0.5f;
        float elapsed = 0f;

        Vector3 startPos = player.transform.position;
        Quaternion startRot = player.transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            player.transform.position = Vector3.Lerp(startPos, hidingPosition.position, t);
            player.transform.rotation = Quaternion.Slerp(startRot, hidingPosition.rotation, t);

            yield return null;
        }

        player.transform.position = hidingPosition.position;
        player.transform.rotation = hidingPosition.rotation;


       
        yield return new WaitForSeconds(0.5f);
        doorAnim.Play("Door_Idle");
   
        isTransitioning=false;
    }


    public IEnumerator OusideCloset()                        //   When  player Outside Closet 
    {  isTransitioning= true;
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);
        Debug.Log("outside Closet");
        insideCloset = false;
        canIntract =false;
        Vector3 startPos = player.transform.position;
        Quaternion startRot = player.transform.rotation;


        
        doorAnim.Play("DoorOpenAndClose");
        yield return new WaitForSeconds(0.5f);
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            player.transform.position = Vector3.Lerp(startPos, exitPosition.position, t);
            player.transform.rotation = Quaternion.Slerp(startRot, exitPosition.rotation, t);

            yield return null;
        }

        player.transform.position = exitPosition.position;
        player.transform.rotation = exitPosition.rotation;
        doorAnim.Play("Door_Idle");
    
       isTransitioning=false;
    }

    public void Interact(InventoryNew invenotory)                       //  IIntractable function To  Intract 
    {
         if(isTransitioning) return;

        if (insideCloset)
        {
            StartCoroutine(OusideCloset());
           
            
        }
        else
        {
            StartCoroutine(InsideCloset());
           
            
        }
    }

    
}

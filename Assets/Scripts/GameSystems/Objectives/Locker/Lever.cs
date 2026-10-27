using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractables
{ 
    float targetRotation = 4f;
   
   private int priority = 1;
   
 
   bool isRotating = false;
   public int Priority => priority;
  
    int maxDigit=4;
   public int currentDigit { get; set; } = 0;
  
   public GameObject leverPivot;
   public float rotationAmount = 70f;
    
  public  Vector3 rotationDir = Vector3.down;
    public void Interact(InventoryNew inventory)
    {
     isRotating= true;
      currentDigit = (currentDigit + 1) % maxDigit;
        Debug.Log("Lever Interacted");    
        StartCoroutine(RotateLever());

    }
  
    IEnumerator RotateLever()
    {
        float rotationSpeed = 100f;
       // float rotationAmount = 70f; 
        float rotatedForward = 0f;
   
        while (rotatedForward < rotationAmount)
        {
            float step = rotationSpeed * Time.deltaTime;
            transform.Rotate(-rotationDir/*Vector3.up*/, step);
            rotatedForward += step;
        }
          yield return new WaitForSeconds(0.5f);
       
        float rotatedBackward = 0f;
        while (rotatedBackward < rotationAmount)
        {
            float step = rotationSpeed * Time.deltaTime;
            transform.Rotate(rotationDir/*Vector3.down*/, step);
            rotatedBackward += step;
        } 
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        isRotating = false;
    }
   
}

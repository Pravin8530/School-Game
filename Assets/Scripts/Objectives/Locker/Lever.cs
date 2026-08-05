using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractables
{ 
    float targetRotation = 4f;
   
   private int priority = 1;
   private float rotationSpeed = 5f;
 
   bool isRotating = false;
   public int Priority => priority;
  
    int maxDigit=4;
   public int currentDigit { get; set; } = 0;

    void Update()
    {
    //    if (isRotating)
    //    {
    //       // transform.Rotate(0f, targetRotation * Time.deltaTime * rotationSpeed, 0f);
          
    //    }


    }

    // Update is called once per frame
    public void Interact()
    {
     isRotating= true;
      currentDigit = (currentDigit + 1) % maxDigit;
        Debug.Log("Lever Interacted");

    }

    public void Drop()
    {
        

    }
}

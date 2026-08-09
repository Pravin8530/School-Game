using System.Threading;
using UnityEngine;

public class LockerDial : MonoBehaviour, IInteractables
{

    float timer = 0f;

   [SerializeField] private float targetRotation = 50f;
    
   float speed =5f;
    [SerializeField] private float timeBetweenRotations = 1f;

    public GameObject lockerDial;
    public bool isRotating = false;
    public int Priority => 1;
    
    public int RotationDirection = 1;

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
            RotateDial();
        
    }

    public void Interact()
    {  
        Debug.Log("Dial Interacted");
        
        if(isRotating)
        {
            Drop();
        }
        else
        {
            isRotating = true;
        }

    }


    private void RotateDial()
    { 
        lockerDial.transform.Rotate(0f, 0f, speed * targetRotation * RotationDirection * Time.deltaTime );

    }

    public void Drop()
    {
       isRotating = false;
        float currentYRotation = lockerDial.transform.localEulerAngles.z;
    }

}


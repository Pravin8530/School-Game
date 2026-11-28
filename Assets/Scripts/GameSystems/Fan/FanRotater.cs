using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class FanRotater : MonoBehaviour, IInteractables
{
    private int priority = 1;
    public int Priority =>priority;
   
   public float duriation = 0.5f;
   private Quaternion currentRotation;
   private Quaternion targetRotation;
   [SerializeField] private GameObject fan;
    
    [SerializeField] private bool isRotating;
    public FanController fancontroller;
  
    void Start()
    { 

        currentRotation = transform.localRotation;
    }

    void Update()
    {
        fan.transform.Rotate(0f,0f,fancontroller.GetSpeed()* Time.deltaTime);
    }

    public void Interact(InventoryNew inventory)
    {  
        if(isRotating) return;
         StartCoroutine(FanRotationRutine());

    }


    private IEnumerator FanRotationRutine()
    {  
        isRotating =true;
       float timeElasped = 0f;

        currentRotation = transform.localRotation;
        targetRotation =  Quaternion.Euler(0f, fancontroller.GetAngle(),0f);

        while (timeElasped < duriation)
        {
             timeElasped += Time.deltaTime;
             float t = timeElasped/duriation;
             transform.localRotation= Quaternion.Slerp(currentRotation,targetRotation,t);
             yield return null;

        }
        transform.localRotation = targetRotation;
       currentRotation = targetRotation;
     isRotating= false;
    }
}

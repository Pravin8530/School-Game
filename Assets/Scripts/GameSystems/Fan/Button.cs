using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Button : MonoBehaviour, IInteractables
{
    private int priority = 1;
    public int Priority => priority;
    public float duriation = 0.1f;
    private Vector3 startPos;
    public FanController fancontroller;
    public int buttonIndex;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Interact(InventoryNew inventory)
    {

        StartCoroutine(ButtonPressRoutine());
           fancontroller.FanSpeed(buttonIndex);

    }

      public IEnumerator ButtonPressRoutine()
    {      

         float elaspedTime = 0f;
        startPos = transform.position;
        Vector3 endPos = startPos - new Vector3(0f, 0.1f, 0f);

        while (elaspedTime < duriation)
        {
            elaspedTime += Time.deltaTime;
            float t = elaspedTime / duriation;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;

        }
       elaspedTime =0f;
      transform.position = endPos;
        while(elaspedTime < duriation)
        {
            elaspedTime += Time.deltaTime;
            float t = elaspedTime/duriation;
            transform.position = Vector3.Lerp(endPos,startPos,t);
            yield return null;

        } 
        
        startPos =transform.position;
      


    }

}

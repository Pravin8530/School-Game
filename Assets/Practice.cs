using System;
using UnityEngine;

public class Practice : MonoBehaviour
{
     public GameObject targetLocation;
 
     public Vector3 smoothDampVelocity = Vector3.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        //MoveWIthTransform();
       //MoveWithTranslate();
      // MoveSmoothDamp();
    }

    void MoveWIthTransform()
    {
        
         Vector3 dir = targetLocation.transform.position - transform.position;
         
        Vector3 dirOnly = dir.normalized;  

         transform.position += dirOnly * 5f  * Time.deltaTime;
    }

    void MoveWithTranslate()
    {
        Vector3 dir = targetLocation.transform.position - transform.position;
         transform.Translate(dir *5f* Time.deltaTime);
    }
 
   void MoveSmoothDamp()
     {
          transform.position = Vector3.SmoothDamp(
               transform.position,
               targetLocation.transform.position,
               ref smoothDampVelocity,
               10f * Time.deltaTime);
          
    }




}

using System;
using System.Numerics;
using UnityEditor.Callbacks;
using UnityEngine;

public class RopeMovement : MonoBehaviour
{
    public FanController fanController;
    public Rigidbody[] joints;
    public float force =20f;
    public float maxSpeed =1200f;
    void Start()
    {
        
    }

    void FixedUpdate()
    { 
         float  speed = fanController.GetSpeed();
         float normalizeSpeed = Mathf.Clamp01(speed/maxSpeed);

        if (fanController.isFacingRope)
        {   
            foreach(Rigidbody joint in joints)
            {
                Rigidbody rb = joint.GetComponent<Rigidbody>();

                 rb.AddForce(transform.forward * normalizeSpeed* force);
                
            }

        }

    }
}

using UnityEngine;

public class Book : MonoBehaviour, IInteractables
{
    private int priority = 1;
    public int Priority => priority;

    public float moveSpeed = 10f;


    private Rigidbody rb;

    private bool isWatching;

    public Vector3 offset ;
     private Vector3 originPos; 

     
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originPos= transform.position;

    }

    void Update()
    {
       
       Vector3 targetPos = isWatching?originPos + offset: originPos;

       transform.position =  Vector3.Lerp(transform.position, targetPos,2f);
         
       

    }

    public void Interact()
    {
        isWatching = !isWatching;

    }
}
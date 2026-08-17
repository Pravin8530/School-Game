using UnityEngine;

public class Magnet : MonoBehaviour, IInteractables
{
    
    public int priority = 1;

    public int Priority => priority;
    private InventoryNew inventory;
    public Transform hand;
   
    public bool hasMagnet;
 
    //public Transform sphere;
   
 
  public Transform target;

    void Awake()
    {
        inventory = hand.GetComponentInParent<InventoryNew>();

    }

    // Update is called once per frame
    private void Update()
    {
        if (hasMagnet)
        {
            MoveSphere();
        }
    }

    public void Interact()
    { 
        Debug.Log("Interacted");
        Transform magnet = hand.Find("magnet");

        if (magnet == null)
        {
            Debug.LogError("magnet not found!");
            return;
        }      

        inventory.RemoveHeldItem(magnet.gameObject);
        magnet.SetParent(null);

        hasMagnet = true;

    }

    private void MoveSphere()
    {
     

        Debug.Log("Moving Sphere");
       transform.position = Vector3.MoveTowards(transform.position, Vector3.left, 0.05f * Time.deltaTime);

    }

}

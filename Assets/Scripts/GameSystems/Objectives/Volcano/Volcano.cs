using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;

public class Volcano : MonoBehaviour, IInteractables
{
    public int priority = 1;
    public int Priority => priority;

    public GameObject volcano;

    private ParticleSystem volcanoPartical;
    public GameObject elementComponent;
    private Rigidbody rb;

    private InventoryNew inventory;

    private float throwSpeed = 15f;

    void Start()
    {
        volcanoPartical = volcano.GetComponent<ParticleSystem>();
        rb = elementComponent.GetComponent<Rigidbody>();
        if (volcanoPartical == null)
            Debug.Log("no partical assigned");

    }



    public void Interact(InventoryNew inventory)
    {
        ItemData mentos = inventory.HeldItem;

        if (mentos == null || mentos.itemName != "mentos")
            return;

        GameObject mentosObject = inventory.HeldObject;
   
        if (mentosObject == null)
            return;

        inventory.RemoveHeldItem(mentosObject);

        Transform itemTransform = mentosObject.transform;
        itemTransform.SetParent(null);

        // Drop Mentos from above the volcano.
        itemTransform.position =
            elementComponent.transform.position + Vector3.up * 2f;

        Rigidbody mentosRb = mentosObject.GetComponent<Rigidbody>();

        if (mentosRb != null)
        {
            mentosRb.linearVelocity = Vector3.zero;
            mentosRb.angularVelocity = Vector3.zero;
            mentosRb.transform.SetParent(null);
          
        }

        // Trigger the eruption.
        if (volcanoPartical != null)
            volcanoPartical.Play();

        if (elementComponent != null)
        {
            Debug.Log("elementComponent : " + rb);
           rb.isKinematic = false;
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(Vector3.up * throwSpeed, ForceMode.Impulse);
        }
    }
}

using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;
using System.Collections;

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

          [Header("Target Setup")]
public Transform targetLandingSpot; // 👈 Drag your target destination GameObject here
public float flyDuration = 1.5f;    // Time it takes to fly there
public float arcHeight = 4f;        // How high it loops up


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
          //   rb.AddForce(Vector3.up * throwSpeed, ForceMode.Impulse);
            StartCoroutine(ArcToPosition());
        }

  

    }
    private IEnumerator ArcToPosition()
{
    rb.isKinematic = true; // Turn off physics completely during the trip
    
    Vector3 startPos = elementComponent.transform.position;
    Vector3 endPos = targetLandingSpot.position;
    float timeElapsed = 0f;

    while (timeElapsed < flyDuration)
    {
        timeElapsed += Time.deltaTime;
        float t = timeElapsed / flyDuration;

        // Linearly interpolate horizontal space
        Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);

        // Add a mathematical parabolic arc to the Y height
        currentPos.y += Mathf.Sin(t * Mathf.PI) * arcHeight;

        elementComponent.transform.position = currentPos;
        yield return null;
    }

    elementComponent.transform.position = endPos; // Precise hard snap
    rb.isKinematic = false; // Turn physics back on when it lands if needed
}

}

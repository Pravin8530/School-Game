using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;

public class Volcano : MonoBehaviour, IInteractables
{
    public int priority = 1;
    public int Priority => priority;

    public GameObject hand;
    public GameObject volcano;

    private ParticleSystem volcanoPartical;
    public GameObject elementComponent;
    private Rigidbody rb;

    private InventoryNew inventory;

    void Start()
    {
        volcanoPartical = volcano.GetComponent<ParticleSystem>();
        rb = elementComponent.GetComponent<Rigidbody>();
        inventory = hand.GetComponentInParent<InventoryNew>();
    }

    // Update is called once per frame
   

    public void Interact()
    {
        Transform mentos = hand.transform.Find("mentos");

        if (mentos != null && mentos.gameObject.activeInHierarchy)
        {
            Debug.Log("InteractedVolcano");

            // Remove Mentos from inventory
            inventory.RemoveHeldItem(mentos.gameObject);

            // Remove from player's hand
            mentos.SetParent(null);

            // Enable physics
            Rigidbody mentosRb = mentos.GetComponent<Rigidbody>();

            if (mentosRb != null)
            {
                mentosRb.isKinematic = false;
                mentosRb.useGravity = true;

                // Put it inside/above volcano
                mentos.position = elementComponent.transform.position;

                // Drop it into volcano
                mentosRb.AddForce(Vector3.down * 1f, ForceMode.Impulse);
            }

            // Volcano effect
            volcanoPartical.Play();

            // Volcano element launches
            rb.isKinematic = false;
            rb.AddForce(Vector3.up * 11f, ForceMode.Impulse);
        }
    }
}

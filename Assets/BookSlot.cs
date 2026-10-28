using UnityEngine;
using System.Collections;

public class BookSlot : MonoBehaviour, IInteractables
{
    public int Priority => 1;

    [Header("References")]
    public Transform KeyPoint;
    // public Transform spinningPart;
    // public Transform gate;

    [Header("Wheel")]
    public float spinDuration = 2f;
    public float rotationAmount = 360f;
    public Vector3 rotationAxis ;

    [Header("Gate")]
    public float gateOpenHeight = 3f;
    public float gateOpenDuration = 3f;

    private bool bookInstalled;
    // private bool isSpinning;
    // private bool gateOpened;
    public GameObject bookShelf;

    public void Interact(InventoryNew inventory)
    {
        if (inventory == null)
            return;

        
        if (!bookInstalled)
        {
            ItemData itemData = inventory.HeldItem;

            if (itemData == null || itemData.itemName != "Book")
            {
                return;
            }

            if (KeyPoint == null)
            {
                return;
            }

            GameObject Book = inventory.HeldObject;

            if (Book == null)
                return;

            inventory.RemoveHeldItem(Book);

            // Rigidbody rb = Book.GetComponent<Rigidbody>();
            // if (rb != null)
            // {
            //     Destroy(rb);
            // }

            Collider collider = Book.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger =true;
            }

            Book.transform.SetParent(KeyPoint);
            Book.transform.localPosition = Vector3.zero;
            Book.transform.localRotation = Quaternion.identity;
            
            bookInstalled = true;
            Book.transform.SetParent(bookShelf.transform);
             Pickable pickableScript = Book.GetComponent<Pickable>();
             pickableScript.enabled = false;
            return;
        }

    //     if (isSpinning || gateOpened)
    //         return;

    //     if (spinningPart == null)
    //     {
    //         Debug.LogError("Assign the actual rotating wheel mesh to Spinning Part.");
    //         return;
    //     }

    //     if (gate == null)
    //     {
    //         Debug.LogError("Assign Gate in Inspector.");
    //         return;
    //     }

    //     StartCoroutine(SpinMechanism());
    // }

    // private IEnumerator SpinMechanism()
    // {
    //     isSpinning = true;

        
    
    //     float elapsed = 0f;

    //     float spinSpeed = rotationAmount / spinDuration;
    //     Vector3 startPosition = gate.position;
    //     Vector3 endPosition = startPosition + Vector3.forward * gateOpenHeight;
    //     while (elapsed < spinDuration)
    //     {
    //         elapsed += Time.deltaTime;

    //          spinningPart.transform.Rotate(rotationAxis * spinSpeed * Time.deltaTime, Space.Self);
    //         //gate
    //           float t = Mathf.Clamp01(elapsed / spinDuration);
    //         gate.position = Vector3.Lerp(startPosition, endPosition, t);


    //         yield return null;
    //     }


        
    }

    
}

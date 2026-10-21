using UnityEngine;
using System.Collections;

public class SpiningHandle : MonoBehaviour, IInteractables
{
    public int Priority => 1;

    [Header("References")]
    public Transform handlePoint;
    public Transform spinningPart;
    public Transform gate;

    [Header("Wheel")]
    public float spinDuration = 2f;
    public float rotationAmount = 360f;
    public Vector3 rotationAxis = Vector3.forward;

    [Header("Gate")]
    public float gateOpenHeight = 3f;
    public float gateOpenDuration = 3f;

    private bool handleInstalled;
    private bool isSpinning;
    private bool gateOpened;

    public void Interact(InventoryNew inventory)
    {
        if (inventory == null)
            return;

        Debug.Log("Wheel mechanism interacted.");

        if (!handleInstalled)
        {
            ItemData itemData = inventory.HeldItem;

            if (itemData == null || itemData.itemName != "Spining Handle")
            {
                return;
            }

            if (handlePoint == null)
            {
                return;
            }

            GameObject handle = inventory.HeldObject;

            if (handle == null)
                return;

            inventory.RemoveHeldItem(handle);

            Rigidbody rb = handle.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Destroy(rb);
            }

            Collider collider = handle.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            handle.transform.SetParent(handlePoint);
            handle.transform.localPosition = Vector3.zero;
            handle.transform.localRotation = Quaternion.identity;

            handleInstalled = true;

            Debug.Log("Handle installed. Interact with the mechanism again.");
            return;
        }

        if (isSpinning || gateOpened)
            return;

        if (spinningPart == null)
        {
            Debug.LogError("Assign the actual rotating wheel mesh to Spinning Part.");
            return;
        }

        if (gate == null)
        {
            Debug.LogError("Assign Gate in Inspector.");
            return;
        }

        StartCoroutine(SpinMechanism());
    }

    private IEnumerator SpinMechanism()
    {
        isSpinning = true;

        
    
        float elapsed = 0f;

        float spinSpeed = rotationAmount / spinDuration;
        Vector3 startPosition = gate.position;
        Vector3 endPosition = startPosition + Vector3.up * gateOpenHeight;
        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;

             transform.Rotate(rotationAxis * spinSpeed * Time.deltaTime, Space.Self);
            //gate
              float t = Mathf.Clamp01(elapsed / spinDuration);
            gate.position = Vector3.Lerp(startPosition, endPosition, t);


            yield return null;
        }


        
    }

    
}

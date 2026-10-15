using UnityEngine;
using System.Collections;

public class SpiningHandle2 : MonoBehaviour, IInteractables
{
    public int Priority => 1;

    [Header("References")]
    public Transform handlePoint;
    public Transform spinningPart; // Note: Since you're using transform.Rotate, this script spins itself
    public Transform targetObject;  // The second object you want to rotate instead of the gate

    [Header("Wheel (Self)")]
    public float spinDuration = 2f;
    public float rotationAmount = 360f;
    public Vector3 rotationAxis = Vector3.forward;

    [Header("Target Object Rotation")]
    public float targetObjectRotation = 90f; // How many degrees the target object should turn on Z

    private bool handleInstalled;
    private bool isSpinning;
    private bool mechanismCompleted;

    public void Interact(InventoryNew inventory)
    {
        if (inventory == null)
            return;

        Debug.Log("Wheel mechanism interacted.");

        // FIRST INTERACTION: install handle.
        if (!handleInstalled)
        {
            ItemData itemData = inventory.HeldItem;

            if (itemData == null || itemData.itemName != "Spining Handle") return;
            if (handlePoint == null) return;

            GameObject handle = inventory.HeldObject;
            if (handle == null) return;

            inventory.RemoveHeldItem(handle);

            Rigidbody rb = handle.GetComponent<Rigidbody>();
            if (rb != null) Destroy(rb);

            Collider collider = handle.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;

            handle.transform.SetParent(handlePoint);
            handle.transform.localPosition = Vector3.zero;
            handle.transform.localRotation = Quaternion.identity;

            handleInstalled = true;
            Debug.Log("Handle installed. Interact with the mechanism again.");
            return;
        }

        // SECOND INTERACTION: spin wheel and rotate the target object.
        if (isSpinning || mechanismCompleted)
            return;

        if (targetObject == null)
        {
            Debug.LogError("Assign the Target Object in the Inspector.");
            return;
        }

        StartCoroutine(SpinMechanism());
    }

    private IEnumerator SpinMechanism()
    {
        isSpinning = true;
        float elapsed = 0f;

        // How fast this wheel spins per second
        float spinSpeed = rotationAmount / spinDuration;

        // Cache the target object's starting and ending Z rotations
        Quaternion startRotation = targetObject.localRotation;
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(targetObjectRotation, Vector3.forward);

        while (elapsed < spinDuration)
        {
            if (targetObject == null)
            {
                isSpinning = false;
                yield break;
            }

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / spinDuration);

            // 1. Spins this wheel around its own local axis every frame
            transform.Rotate(rotationAxis * spinSpeed * Time.deltaTime, Space.Self);

            // 2. Rotate the target object around its local Z-axis cleanly using the 0-1 percentage
            targetObject.localRotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        // Hard snap the target object to its exact final rotation to wipe out rounding math errors
        if (targetObject != null)
            targetObject.localRotation = endRotation;

        mechanismCompleted = true;
        isSpinning = false;

        Debug.Log("Mechanism completed! Both objects finished rotating.");
    }
}

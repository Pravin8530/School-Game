using UnityEngine;
using System.Collections;

public class SpiningHandle2 : MonoBehaviour, IInteractables
{
    public int Priority => 1;

    [Header("References")]
    public Transform handlePoint;
    public Transform spinningPart;
    public Transform targetObject;
    [Header("Wheel (Self)")]
    public float spinDuration = 2f;
    public float rotationAmount = 360f;
    public Vector3 rotationAxis = Vector3.forward;

    [Header("Target Object Rotation")]
    public float targetObjectRotation = 90f;

    private bool handleInstalled;
    private bool isSpinning;
    private bool mechanismCompleted;
    [SerializeField] private bool requireDoubleinteraction;

    public void Interact(InventoryNew inventory)
    {
        if (inventory == null)
            return;


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
            if (requireDoubleinteraction == false)
            {
                StartCoroutine(SpinMechanism());
            }
            return;
        }


        if (isSpinning || mechanismCompleted)
            return;

        if (targetObject == null)
        {
            return;
        }

        if (requireDoubleinteraction == true)
        {
            StartCoroutine(SpinMechanism());
        }

    }

    private IEnumerator SpinMechanism()
    {
        isSpinning = true;
        float elapsed = 0f;

        float spinSpeed = rotationAmount / spinDuration;

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

            transform.Rotate(rotationAxis * spinSpeed * Time.deltaTime, Space.Self);

            targetObject.localRotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }

        if (targetObject != null)
            targetObject.localRotation = endRotation;

        mechanismCompleted = true;
        isSpinning = false;

    }
}

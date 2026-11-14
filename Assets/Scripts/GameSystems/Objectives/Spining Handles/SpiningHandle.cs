using UnityEngine;
using System.Collections;

public class SpiningHandle : MonoBehaviour, IInteractables
{
    public int Priority => 1;

    public enum GateDirection
    {
        forward,
        backward,
        up,
        down,
        left,
        right
    }
    [Header("Target Object Rotation")]
    public float targetObjectRotation = 90f;
    public bool shouldRotateTargetObject = false;
    public Vector3 targetRotationAxis = Vector3.forward;
    [Header("References")]
    public Transform handlePoint;
    public Transform spinningPart;
    public Transform gate;
    public Transform targetObject;

    [Header("Wheel")]
    public float spinDuration = 2f;
    public float rotationAmount = 360f;
    public Vector3 rotationAxis = Vector3.forward;

    public bool requireDoubleInteraction = false;

    [Header("Gate")]
    public float gateOpenHeight = 3f;
    public float gateOpenDuration = 3f;

    private bool handleInstalled;
    private bool isSpinning;
    private bool gateOpened;
    public GateDirection gateDirection = GateDirection.forward;
    public Vector3[] openDirection = { Vector3.forward, Vector3.back, Vector3.up, Vector3.down, Vector3.left, Vector3.right };
    public string interactingObjectName = "Spining Handle";
  
  
    public void Interact(InventoryNew inventory)
    {
        if (inventory == null)
            return;


        if (!handleInstalled)
        {
            ItemData itemData = inventory.HeldItem;

            if (itemData == null || itemData.itemName != interactingObjectName)
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
            if (requireDoubleInteraction == false)
            {  if (shouldRotateTargetObject == true)
                {
                    StartCoroutine(RotateTargetSpinMechanism());
                }
                else
                {
                    StartCoroutine(SpinMechanism());
                }
               
            }
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
            return;
        }
        if (shouldRotateTargetObject==true && spinningPart != null)
        {
            StartCoroutine(RotateTargetSpinMechanism());
        }
        if (requireDoubleInteraction ==true && shouldRotateTargetObject == false)
        {
            StartCoroutine(SpinMechanism());
        }
    }

    private IEnumerator SpinMechanism()
    {
        isSpinning = true;
        float elapsed = 0f;

        float spinSpeed = rotationAmount / spinDuration;
        Vector3 startPosition = gate.localPosition;
        Vector3 endPosition = startPosition + openDirection[(int)gateDirection] * gateOpenHeight;
        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;

            transform.Rotate(rotationAxis * spinSpeed * Time.deltaTime, Space.Self);
            //gate
            float t = Mathf.Clamp01(elapsed / spinDuration);
            gate.localPosition = Vector3.Lerp(startPosition, endPosition, t);


            yield return null;
        }

    }


     private IEnumerator RotateTargetSpinMechanism()
    {
        isSpinning = true;
        float elapsed = 0f;

        float spinSpeed = rotationAmount / spinDuration;

        Quaternion startRotation = targetObject.localRotation;
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(targetObjectRotation, targetRotationAxis);

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

        //mechanismCompleted = true;
        isSpinning = false;

    }


}

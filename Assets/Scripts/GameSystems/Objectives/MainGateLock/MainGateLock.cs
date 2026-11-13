using UnityEngine;
using System.Collections;

public class MainGateLock : MonoBehaviour, IInteractables
{ 

   public enum GateDirection
    {
        forward,
        backward,
        up,
        down,
        left,
        right
    }

    public int Priority => 1;

    [Header("References")]
    public Transform KeyPoint;
    public Transform spinningPart;
    public Transform gate;
    [Header("Wheel")]
    public float spinDuration = 2f;
    public float rotationAmount = 360f;
    public Vector3 rotationAxis ;
    public GateDirection gateDirection = GateDirection.forward;
    public Vector3[] openDirection = { Vector3.forward, Vector3.back, Vector3.up, Vector3.down, Vector3.left, Vector3.right };

    [Header("Gate")]
    public float gateOpenPosition = 3f;
    public float gateOpenDuration = 3f;

    private bool handleInstalled;
    private bool isSpinning;
    private bool gateOpened;
 
    [SerializeField] private bool requireDoubleinteraction;

    public void Interact(InventoryNew inventory)
    {
        if (inventory == null)
            return;


        if (!handleInstalled)
        {
            ItemData itemData = inventory.HeldItem;

            if (itemData == null || itemData.itemName != "MainGateKey")
            {
                return;
            }

            if (KeyPoint == null)
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

            handle.transform.SetParent(KeyPoint);
            handle.transform.localPosition = Vector3.zero;
            handle.transform.localRotation = Quaternion.identity;

            handleInstalled = true;
            if (requireDoubleinteraction == false)
            {
                StartCoroutine(SpinMechanism());
            }
            return;
        }

        if (isSpinning || gateOpened)
            return;

        if (spinningPart == null)
        {
            return;
        }

        if (gate == null)
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
        Vector3 startPosition = gate.localPosition;
        Vector3 endPosition = startPosition + openDirection[(int)gateDirection] * gateOpenPosition;
        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;

             spinningPart.transform.Rotate(rotationAxis * spinSpeed * Time.deltaTime, Space.Self);
            //gate
              float t = Mathf.Clamp01(elapsed / spinDuration);
            gate.localPosition = Vector3.Lerp(startPosition, endPosition, t);

            yield return null;
        }
        gate.localPosition = endPosition;
        gateOpened = true;
        isSpinning = false;    
    }

    
}

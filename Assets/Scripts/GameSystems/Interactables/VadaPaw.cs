using UnityEngine;
using System.Collections;

public class VadaPaw : MonoBehaviour, IInteractables
{
    public int Priority => 1;

    //[Header("References")]
     public Transform VadaPoint;
   

    [Header("Wheel")]
    public float spinDuration = 2f;
    public float rotationAmount = 360f;
    public Vector3 rotationAxis = Vector3.forward;

     public TeacherController teacherController;

     private bool VadaInstalled;
 

    public void Interact(InventoryNew inventory)
    {
        if (inventory == null)
            return;

        Debug.Log("Wheel mechanism interacted.");

        if (!VadaInstalled)
        {
            ItemData itemData = inventory.HeldItem;

            if (itemData == null || itemData.itemName != "VadaPaw")
            {
                return;
            }

            if (VadaPoint == null)
            {
                return;
            }

            GameObject Vada = inventory.HeldObject;

            if (Vada == null)
                return;

            inventory.RemoveHeldItem(Vada);

            Rigidbody rb = Vada.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Destroy(rb);
            }

            Collider collider = Vada.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            Vada.transform.SetParent(VadaPoint);
            Vada.transform.localPosition = Vector3.zero;
            Vada.transform.localRotation = Quaternion.identity;

            VadaInstalled = true;
            teacherController.ChangeState(new VadaPawState(teacherController, VadaPoint));
            Debug.Log("Vada Placed Successfully");
            return;
        }

        // if (isSpinning || gateOpened)
        //      return;

        // if (spinningPart == null)
        // {
        //     Debug.LogError("Assign the actual rotating wheel mesh to Spinning Part.");
        //     return;
        // }

        // if (gate == null)
        // {
        //     Debug.LogError("Assign Gate in Inspector.");
        //     return;
        // }

       // StartCoroutine(SpinMechanism());
    }

    // private IEnumerator SpinMechanism()
    // {
    //     isSpinning = true;

        
    
    //     float elapsed = 0f;

    //     float spinSpeed = rotationAmount / spinDuration;
    //     Vector3 startPosition = gate.position;
    //     Vector3 endPosition = startPosition + Vector3.up * gateOpenHeight;
    //     while (elapsed < spinDuration)
    //     {
    //         elapsed += Time.deltaTime;

    //          transform.Rotate(rotationAxis * spinSpeed * Time.deltaTime, Space.Self);
    //         //gate
    //           float t = Mathf.Clamp01(elapsed / spinDuration);
    //         gate.position = Vector3.Lerp(startPosition, endPosition, t);


    //         yield return null;
    //     }


        
    // }

    
}

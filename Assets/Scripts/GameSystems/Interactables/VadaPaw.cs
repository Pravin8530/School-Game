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
  
     public GameObject vadapawPrefab;
   

    public void Interact(InventoryNew inventory)
    {
        if (inventory == null)
            return;

    

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
            teacherController.ChangeState(new VadaPawState(teacherController, vadapawPrefab.transform));
            Debug.Log("Vada Placed Successfully");
          
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = transform.rotation;

            GameObject vadapawRef = Instantiate(vadapawPrefab,spawnPosition,spawnRotation);
            Destroy(gameObject);
          
 
            return;
        }
  
       
    }

    

    
}

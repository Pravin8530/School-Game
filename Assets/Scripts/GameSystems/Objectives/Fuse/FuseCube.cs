using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class FuseCube : MonoBehaviour, IInteractables
{
    public int Priority => 1;
    [SerializeField] private int rotateSpeed;
    public Quaternion targetRotation;

    public bool isConected;

    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed);
    }

    public void Interact(InventoryNew invetory)
    {

        targetRotation *= Quaternion.Euler(0, 90, 0);


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("FusePoint"))
         {

            isConected = true;
        }

    }



}

using UnityEngine;

public class FuseCubeNew : MonoBehaviour, IInteractables

{
    public int Priority => 1;
    [SerializeField] private int correctRotationIndex;
    [SerializeField] private int rotateSpeed =5;

    [SerializeField] private int currentRotationIndex;

     [SerializeField] private Quaternion targetRotation;
 
     public bool isConected => currentRotationIndex == correctRotationIndex;
    void Start()
    {
        targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    public void Interact(InventoryNew invetory)
    {    currentRotationIndex = (currentRotationIndex + 1) % 4; // Assuming 4 possible rotations (0, 1, 2, 3)
        targetRotation *= Quaternion.Euler(0, 90, 0);

    }
}

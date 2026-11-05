
using UnityEngine;

public class HidingSpot : MonoBehaviour, IInteractables
{
    [Header("Hiding Positions")]
    [SerializeField] private Transform hidePoint;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform cameraPoint;

    public int Priority => 5;

    public void Interact(InventoryNew inventory)
    {
        HidingController hiding =
            inventory.GetComponent<HidingController>();

        if (hiding == null)
            hiding = inventory.GetComponentInParent<HidingController>();

        if (hiding == null)
        {
            Debug.LogWarning("HidingController not found.");
            return;
        }

        hiding.EnterHiding(
            this, hidePoint, exitPoint, cameraPoint);
    }
}

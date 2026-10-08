using UnityEngine;

public class Paper : MonoBehaviour, IPickable
{
    [SerializeField] private Vector3 holdPosition;
    [SerializeField] private Vector3 rotation;

    // 1. ADD THIS VARIABLE
    [SerializeField] private string paperName = "English Paper";

    private Transform originalParent;
    private Rigidbody rb;
    private Collider col;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    // 2. ADD THIS FUNCTION SO THE PRINTER CAN READ IT
    public string GetPaperName()
    {
        return paperName;
    }

    public void Interact(Transform hand)
    {
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.HasPaper, this);

        InventoryNew inventory = hand.GetComponentInParent<InventoryNew>();
        if (inventory != null)
        {
            inventory.HideEveryObject();
        }

        col.enabled = false;
        rb.useGravity = false;
        transform.SetParent(Camera.main.transform);
        transform.localPosition = holdPosition;
        transform.localRotation = Quaternion.Euler(rotation);
    }
}

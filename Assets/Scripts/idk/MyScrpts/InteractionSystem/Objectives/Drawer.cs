using UnityEngine;

public class Drawer : MonoBehaviour, IInteractables
{
    [SerializeField] private Vector3 openOffset = new Vector3(0, 0, 0.5f);
    [SerializeField] private float moveSpeed = 3f;

    

    private Vector3 startLocalPos;
    private bool isOpen;
    public int Priority => 1;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        HandleOpenClose();
    }

    void HandleOpenClose()
    {
        Vector3 target = startLocalPos + (isOpen ? openOffset : Vector3.zero);

        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            target,
            moveSpeed * Time.deltaTime
        );
    }

    // called by Player system
    public void Interact()
    {
        isOpen = !isOpen;
    }

    public void Drop()
    {
        isOpen = false;
    }
    
}
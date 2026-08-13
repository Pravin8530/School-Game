using UnityEngine;

public class Book : MonoBehaviour, IInteractables
{
    private int priority = 1;
    public int Priority => priority;

    public Vector3 offset;
    public float distance = 1.5f;
    public float moveSpeed = 10f;

    Vector3 originPos;
    Rigidbody rb;
    bool isWatching;

    Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        originPos = transform.position;
    }

    void Update()
    {
        if (isWatching)
        {
            Vector3 targetPos =
                cam.transform.position +
                cam.transform.forward * distance +
                cam.transform.TransformVector(offset);

            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                Time.deltaTime * moveSpeed
            );
        }
        else
        {
            transform.position = Vector3.Lerp(
                transform.position,
                originPos,
                Time.deltaTime * moveSpeed
            );
        }
    }

    public void Interact()
    {
        isWatching = !isWatching;
        rb.isKinematic = isWatching;
    }
}
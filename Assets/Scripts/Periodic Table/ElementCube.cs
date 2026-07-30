using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class ElementCube : MonoBehaviour, IPickable
{

    public ElementType element;

    [SerializeField]
    private Vector3 holdPosition;
    [SerializeField]
    private Vector3 scale;

    [SerializeField]
    private Vector3 rotation;

    private Transform originalParent;
    private Rigidbody rb;
    private Collider col;

    [SerializeField]
    private Vector3 originalScale;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void Drop()
    {
       // throw new System.NotImplementedException();
    }

    public void Interact(Transform hand)
    {

        originalScale = transform.localScale;
        col.enabled = false;
        rb.useGravity = false;
        transform.SetParent(Camera.main.transform);
        transform.localPosition = holdPosition;
        transform.localScale =  scale;
        transform.localRotation = Quaternion.Euler(rotation);

    }
 
   
   
}

public enum ElementType { H, He, Li, Be, B, C, N, O ,Rn }

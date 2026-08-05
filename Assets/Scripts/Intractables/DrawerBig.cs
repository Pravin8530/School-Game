using System;
using NUnit.Framework;
using UnityEngine;

public class DrawerBig : MonoBehaviour, IInteractables
{

    [Header("Bool Value")]
    private bool isOpen;
    [SerializeField] private bool isMoving;

    [Header("Drawer Open Speed")]
    public float speed = 2f;

    [Header("Drawer Vector Position")]
    private Vector3 closeDrawer;
    private Vector3 openDrawer;

    [Header("Collider")]
    public Collider drawerCollider;

    [Header("Raycast Priority")]
    [SerializeField] private int priority = 1;

    [Header("Raycast Priority")]
    public int Priority => priority;

    public Vector3 offset;

    void Start()
    { 
        //offset = new Vector3(0f,0f,0f);
        closeDrawer = transform.localPosition;

        openDrawer = closeDrawer + offset;

        
    }

    void Update()
    {

        HandleOpenClose();

    }

    public void Interact()                    // IIntractable function To  Intract 
    {
        if (isMoving) return;

        drawerCollider.enabled = false;

        isOpen = !isOpen;
        isMoving = true;

    }

    public void HandleOpenClose()
    {
        Vector3 targetPos = isOpen ? openDrawer : closeDrawer;

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, targetPos) < 0.01f)
        {
            transform.localPosition = targetPos;

            isMoving = false;

            drawerCollider.enabled = true;
        }


    }

    public void Drop()
    {
        
    }

}

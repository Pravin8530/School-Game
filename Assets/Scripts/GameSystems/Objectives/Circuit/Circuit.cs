
using UnityEngine;

public class Circuit : MonoBehaviour, IInteractables
{
    public int priority = 1;

    public int Priority => priority;
    private InventoryNew inventory;
    public Transform hand;
    public Transform drawer;
    public bool hasCell;

   
 
  public Transform openDrawerPos;

    void Awake()
    {
        inventory = hand.GetComponentInParent<InventoryNew>();

    }

    private void Update()
    {
        if (hasCell)
        {
            OpenDrawer();
        }
    }

    public void Interact(InventoryNew inventory)
    {
        Transform cell = hand.Find("Cell");
         

        if (cell == null)
        {
            return;
        }

        cell.position = transform.position;
        cell.rotation = transform.rotation;

        inventory.RemoveHeldItem(cell.gameObject);
        cell.SetParent(null);

        hasCell = true;

    }

    private void OpenDrawer()
    {
      
        drawer.position = Vector3.Lerp(drawer.position, openDrawerPos.position, 3f * Time.deltaTime);

    }

}

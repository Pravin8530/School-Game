
using UnityEngine;

public class Circuit : MonoBehaviour, IInteractables
{
    public int priority = 1;

    public int Priority => priority;
    private InventoryNew inventory;
    public Transform hand;
    public Transform drawer;
    public bool hasCell;

   // public Vector3 openDrawerPos;
   // public Vector3 closeDrawerPos;
 
  public Transform openDrawerPos;

    void Awake()
    {
        inventory = hand.GetComponentInParent<InventoryNew>();

    }

    // Update is called once per frame
    private void Update()
    {
        if (hasCell)
        {
            OpenDrawer();
        }
    }

    public void Interact()
    {
        Transform cell = hand.Find("Cell");

        if (cell == null)
        {
            Debug.LogError("Cell not found!");
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
        Debug.Log(
      "Drawer: " + drawer.position +
      " | Target: " + openDrawerPos
  );

        Debug.Log("OpenDrawer");
        drawer.position = Vector3.Lerp(drawer.position, openDrawerPos.position, 5f * Time.deltaTime);

    }

}

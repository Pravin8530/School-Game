using UnityEngine;

public class BookNew : MonoBehaviour,IInteractables
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int Priority => 1;

    public void Interact(InventoryNew inventory)
    {
        //ItemData bookItem = inventory.HeldItem;

        // if (bookItem != null)
        // {
            Debug.Log($"Interacted with book ");
            // Implement logic for interacting with the book while holding another item
        // }
        // else
        // {
            //Debug.Log("Interacted with book while not holding any item.");
            // Implement logic for interacting with the book when not holding any item
        // }
    }
}

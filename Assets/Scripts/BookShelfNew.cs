using UnityEngine;
using System.Collections.Generic;

public class BookShelfNew : MonoBehaviour, IInteractables
{
   public List<Transform> bookSlots = new List<Transform>();
    public int Priority => 1;


// each slot will have state is book or not 
// interacted we loop throgh check  thorugh list if its not there put book  there
    public void Interact(InventoryNew inventory)
    {
     
     ItemData bookItem = inventory.HeldItem;

        if (bookItem != null)
        {
            Debug.Log($"Interacted with bookshelf while holding {bookItem.itemName}.");
            for (int i = 0; i < bookSlots.Count; i++)
            {
               if(bookSlots[i].childCount == 0)
                {
                    // Place the book in the empty slot
                    GameObject bookObject = inventory.heldObjects[inventory.selectedSlot];
                    bookObject.transform.SetParent(bookSlots[i]);
                    bookObject.transform.localPosition = Vector3.zero;
                    bookObject.transform.localRotation = Quaternion.identity;

                    // Remove the book from the player's inventory
                    inventory.RemoveHeldItem(bookObject);

                    Debug.Log($"Placed {bookItem.itemName} in slot {i}.");
                    return;
                }
            }
                                                                             

        }
        else
        {
            Debug.Log("Interacted with bookshelf while not holding a book.");
        }

        




    }

   
}

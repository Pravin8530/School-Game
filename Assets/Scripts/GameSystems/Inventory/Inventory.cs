using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemData> items = new ();

   public void AddItem(ItemData data)
    {
        items.Add(data);

    }
 
   public void RemoveItem(ItemData data)
    {
        items.Remove(data);

    }
  
}

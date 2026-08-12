using UnityEngine;

public class InventoryNew : MonoBehaviour
{ 

   public ItemData[] slots = new ItemData[6];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddItem(ItemData item)
    {
         for(int i =0; i < slots.Length;i++)
        {
            if(slots[i] == null)
            {
                slots[i]=item;
                return true;

            }

        }
           return false;
    }
}

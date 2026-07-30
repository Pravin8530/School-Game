using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public Inventory inventory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Equip(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            Equip(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            Equip(2);
    }

    void Equip(int index)
    {
     

       // if (PlayerInteract.instance.currentPickedItem != null);
            //PlayerInteract.instance.currentPickedItem.Store();

       // PlayerInteract.instance.currentPickedItem.Interact();
    }
}
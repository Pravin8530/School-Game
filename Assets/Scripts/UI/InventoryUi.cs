using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventoryNew inventory;
    public Image[] slotIcons;

    void Update()
    {
        UpdateUI();
    }

 

    void UpdateUI()
    {
        for (int i = 0; i < slotIcons.Length; i++)
        {
            if (inventory.slots[i] != null)
            {
                slotIcons[i].sprite = inventory.slots[i].icon;
            }
            else
            {
                slotIcons[i].sprite = null;
            }
        }
    }
}
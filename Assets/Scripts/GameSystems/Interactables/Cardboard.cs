using StarterAssets;
using System.Collections;
using UnityEngine;

public class Cardboard : MonoBehaviour, IInteractables
{
    public int Priority => 1;
    private bool isHiding;

    // interact
    //box goes in air
    // player scale small
    // lerps to hiding position
    // box drops 
    //interact again box goes up player on exit position normal height 
    

    void Awake()
    {
      
    }

    public void Interact(InventoryNew inventory)
    {

        isHiding = !isHiding;

    }

    public void Hide()
    {


    }

   
   
}
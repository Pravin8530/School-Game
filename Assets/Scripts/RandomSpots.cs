using UnityEngine;
using System.Collections.Generic;
public class RandomSpots : MonoBehaviour
{
//     public GameObject prefab1;
//     public GameObject prefab2;
//    public Transform[] placingSpots=new Transform[8];
  
   // x ammout of prefabs
   // x amout of spots 
   // choose random spot for each prefab
 
  public List<GameObject> prefabs = new List<GameObject>();
  public List<Transform> placingSpots = new List<Transform>();

  
    void Start()
    {
        ChooseRandomSpots();
    }

   
   public void ChooseRandomSpots()
   {
       // Create a list of available spots
       List<Transform> availableSpots = new List<Transform>(placingSpots);

       // Loop through each prefab and assign a random spot
       foreach (GameObject prefab in prefabs)
       {
           if (availableSpots.Count == 0)
           {
               Debug.LogWarning("Not enough spots for all prefabs.");
               break;
           }

           // Choose a random index from the available spots
           int randomIndex = Random.Range(0, availableSpots.Count);
           Transform chosenSpot = availableSpots[randomIndex];

           // Instantiate the prefab at the chosen spot
           Instantiate(prefab, chosenSpot.position, chosenSpot.rotation);

           // Remove the chosen spot from the list to avoid duplicates
           availableSpots.RemoveAt(randomIndex);
       }
   }






//  public void ChooseRandomSpots(){
 
//    int spot1 = Random.Range(0,placingSpots.Length);
//    int spot2;

//    do{

//        spot2 = Random.Range(0,placingSpots.Length);

//    }while(spot2==spot1);

//        Instantiate(prefab1,placingSpots[spot1].position, placingSpots[spot1].rotation);
//        Instantiate(prefab2,placingSpots[spot2].position, placingSpots[spot2].rotation);

//  }

}

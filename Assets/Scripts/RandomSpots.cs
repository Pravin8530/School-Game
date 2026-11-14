using UnityEngine;
using System.Collections.Generic;
public class RandomSpots : MonoBehaviour
{

  public List<GameObject> prefabs = new List<GameObject>();
  public List<Transform> placingSpots = new List<Transform>();

  
    void Start()
    {
        ChooseRandomSpots();
    }

   
   public void ChooseRandomSpots()
   {
       List<Transform> availableSpots = new List<Transform>(placingSpots);

       foreach (GameObject prefab in prefabs)
       {
           if (availableSpots.Count == 0)
           {
               Debug.LogWarning("Not enough spots for all prefabs.");
               break;
           }

           int randomIndex = Random.Range(0, availableSpots.Count);
           Transform chosenSpot = availableSpots[randomIndex];

           Instantiate(prefab, chosenSpot.position, chosenSpot.rotation);

           availableSpots.RemoveAt(randomIndex);
       }
   }






}

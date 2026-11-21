using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FlagSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    public GameObject flagPrefab;

    public float timeToSpawn = 20f;
  
     public float duriation = 20f;
  
    public void Start()
    {
       SpawnFlag();
    }

 

    public void SpawnFlag()
    {
        int randomIndex = Random.Range(0, spawnPoints.Count);
        Transform flagPrefabspot = spawnPoints[randomIndex];

        Instantiate(flagPrefab, flagPrefabspot.position, flagPrefabspot.rotation);
        Debug.Log("flag spawned at :" + flagPrefabspot);
    }

    private void DestroyPreviousFlag()
    {
        GameObject flag = GameObject.FindWithTag("Flag");
        if (flag != null)
        {
            Destroy(flag);
            Debug.Log("destoryed flag " + flag);
        }
    }

    public IEnumerator Delay(float spawntime)
    { 
        yield return new WaitForSeconds(spawntime);
        DestroyPreviousFlag();
        yield return new WaitForSeconds(duriation);
        SpawnFlag();
        timeToSpawn =20f;

    }


    public void SpawnFlagsWithDelay()
    { 
        
        // timeToSpawn -= Time.deltaTime;

        // if (timeToSpawn <= 0)
        // {
        //     DestroyPreviousFlag();
        //     SpawnFlag();
        //     timeToSpawn = 20f;
        // }
       StartCoroutine(Delay(timeToSpawn));
     
    }


}

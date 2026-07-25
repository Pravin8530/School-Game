using UnityEngine;
using System.Collections;
using Unity.Mathematics;
public class Cabin : MonoBehaviour, IInteractables
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //private int priority = 1;
    public int Priority => 1;
    bool isEntered = false;
    public GameObject player;

    public Transform dropPosition;



    public IEnumerator EnterCabin()
    {
        float duration = 0.5f;
        float elapsed = 0f;

        Vector3 startPos = player.transform.position;
        Quaternion startRot = player.transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            player.transform.position = Vector3.Lerp(startPos, dropPosition.position, t);
            player.transform.rotation = Quaternion.Slerp(startRot, dropPosition.rotation, t);

            yield return null;
        }

        player.transform.position = dropPosition.position;
        player.transform.rotation = dropPosition.rotation;
        yield return new WaitForSeconds(0.5f);
        //player.transform.rotation = Quaternion.Slerp(dropPosition.rotation, startRot, 1f);
        StartCoroutine(RotateToOrigin(dropPosition.rotation, player.transform.rotation, 3f));

    }

    private IEnumerator
    RotateToOrigin(quaternion from, quaternion to, float duriation)
    {
        float timeGoneby = 0f;

        while (timeGoneby < duriation)
        {
            timeGoneby += Time.deltaTime;
            float t = timeGoneby / duriation;

            player.transform.rotation = Quaternion.Slerp(from, to, t);

            yield return null;
            
        }
        
            player.transform.rotation = to;

    }




    public void Interact()
    {
        Debug.Log("Cabin Interact");
        if (!isEntered)
        {
            StartCoroutine(EnterCabin());
            isEntered = true;
        }
    }
}

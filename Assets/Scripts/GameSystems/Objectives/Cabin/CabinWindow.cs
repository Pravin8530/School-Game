using UnityEngine;
using System.Collections;
public class CabinWindow : MonoBehaviour, IInteractables
{
   public int Priority => 1;
   bool isEntered = false;
   public GameObject player;

   public Transform dropPosition;

    public Color color = new Color(0, 1, 0, 0.3f);

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
       player.transform.rotation = Quaternion.Slerp(dropPosition.rotation, startRot, 1f);

   }




   public void Interact(InventoryNew inventory)
   {
       Debug.Log("Cabin Interact");
       if (!isEntered)
       {
           StartCoroutine(EnterCabin());
           isEntered = true;
       }
   }

  
    private void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        if (box != null)
        {
            Gizmos.color = color;

            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawCube(box.center, box.size);
        }
    }
}

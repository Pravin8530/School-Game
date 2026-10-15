using StarterAssets;
using UnityEngine;


public class CabinTest : MonoBehaviour
{ 
        public Color color = new Color(0, 1, 0, 0.3f);

   public FirstPersonController FirstPersonController;

  
    

     private void OnTriggerEnter(Collider other)
     {
       if( other.gameObject.CompareTag("Player"))
       {

            Debug.Log("Trigger");
            FirstPersonController.TriggerFall();

            this.GetComponent<Collider>().enabled= false;
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

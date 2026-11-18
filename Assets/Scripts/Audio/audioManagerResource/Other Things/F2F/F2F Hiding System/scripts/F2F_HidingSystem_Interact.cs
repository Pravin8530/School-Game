using UnityEngine;

public class F2F_HidingSystem_Interact : MonoBehaviour
{


    public bool CanInteract = true;

    public F2F_HidingSystem hidingmanager_script;


    // Update is called once per frame
    void Update()
    {


         
        if(Input.GetKeyDown(KeyCode.Space) && hidingmanager_script.InsideCloset == true)
        {

            CanInteract = true;
            StartCoroutine(hidingmanager_script.GoOutCloset_CO());

        }

        if (Input.GetMouseButtonDown(0) && CanInteract == true)
        {

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 5))
            {

                if (hit.collider.CompareTag("Closet"))
                {

                    CanInteract = false;
                    StartCoroutine(hidingmanager_script.GoInsideCloset_CO());
                }

            }


        }


    }
}

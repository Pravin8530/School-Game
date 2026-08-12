using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ElementSlot : MonoBehaviour, IInteractables
{

    public ElementType requiredElement;

    public bool isFilled;

    public Transform GhostelementPosition;

    public int Priority => 1;

    public float zOffset = -3; //

    void Awake()
    {

    }


    public void PlaceElementIntoDedicatedPosition(ElementCube cube)
    {

        // Parent directly to the ghost slot, WITHOUT keeping world position
        cube.transform.SetParent(GhostelementPosition, false);

        // Zero everything out locally — guaranteed exact, no matrix math needed
        cube.transform.localPosition = Vector3.zero;
        cube.transform.localRotation = Quaternion.identity;
        cube.transform.localScale = Vector3.one; // adjust if cube needs to be smaller/larger than the slot
        cube.transform.localPosition = new Vector3(zOffset, 0f, 0f);
        //cube.transform.position = new Vector3(cube.transform.position.x + zOffset, cube.transform.position.y, cube.transform.position.z );

        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        cube.GetComponent<Collider>().enabled = true;
        isFilled = true;

        PeriodicTableCollapse.instance.CheckCompletion();

        // PlayerInteract.instance.ClearHeldItem();
    }

    public void Interact()
    {

        if (isFilled)
        {
            return;
        }


        // ElementCube cube = PlayerInteract.instance.targetPickable as ElementCube;
        ElementCube cube = PlayerInteract.instance.heldElement;
        if (cube == null)
        {
            Debug.Log("cube is null");

        }

        // if (cube != null && cube.element == requiredElement)
        // {

        //     PlaceElementIntoDedicatedPosition(cube);
        // }

        if (cube != null && cube.element == requiredElement)
        {
            PlaceElementIntoDedicatedPosition(cube);
            PlayerInteract.instance.heldElement = null;
        }
    }




}

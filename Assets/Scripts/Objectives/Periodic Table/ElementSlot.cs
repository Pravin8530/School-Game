using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ElementSlot : MonoBehaviour, IInteractables
{

    public ElementType requiredElement;

    public bool isFilled;

    public Transform GhostelementPosition;

    public int Priority => 1;

    public float zOffset = -3; 

    void Awake()
    {

    }


    public void PlaceElementIntoDedicatedPosition(ElementCube cube)
    {

        cube.transform.SetParent(GhostelementPosition, false);

        cube.transform.localPosition = Vector3.zero;
        cube.transform.localRotation = Quaternion.identity;
        cube.transform.localScale = Vector3.one; 
        cube.transform.localPosition = new Vector3(zOffset, 0f, 0f);

        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        cube.GetComponent<Collider>().enabled = true;
        isFilled = true;

        PeriodicTableCollapse.instance.CheckCompletion();

    }

    public void Interact(InventoryNew inventory)
    {

        if (isFilled)
        {
            return;
        }


        ElementCube cube = PlayerInteract.instance.heldElement;
        if (cube == null)
        {
            Debug.Log("cube is null");

        }

      

        if (cube != null && cube.element == requiredElement)
        {
            PlaceElementIntoDedicatedPosition(cube);
            PlayerInteract.instance.heldElement = null;
            PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);

        }


    }




}

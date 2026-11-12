using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class FuseNew : MonoBehaviour
{
    public FuseCubeNew[] cubes;
    public LineRenderer lineRenderer;
    public GameObject Drawer;
    public bool isSolved;
    public Transform DrawerOpenPosition;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ConnectedCheak();
       
    }



    public void ConnectedCheak()
    {
        foreach (FuseCubeNew cube in cubes)
        {
            if (cube == null)
            {
                Debug.LogError("Cube Missing");
                return;
            }

            if (!cube.isConected)
                return;
        }
        isSolved =true;
        Debug.Log("Woooooooo Patern Complete");
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;

        lineRenderer.positionCount = cubes.Length;

        for (int i = 0; i < cubes.Length; i++)
        {
            lineRenderer.SetPosition(i, cubes[i].transform.position);
        }
    }
 
 
      public  void OpenDrawer(){
       
        if (isSolved)
        {
            Drawer.transform.localPosition = Vector3.Lerp(Drawer.transform.localPosition, DrawerOpenPosition.localPosition, Time.deltaTime * 10f);
        }
       else
       {
           return;
       }
     }


}

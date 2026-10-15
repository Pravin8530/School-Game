using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class FuseBox : MonoBehaviour
{
    public FuseCube[] cubes;
    public LineRenderer lineRenderer;
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
        foreach (FuseCube cube in cubes)
        {
            if (cube == null)
            {
                Debug.LogError("Cube Missing");
                return;
            }



            if (!cube.isConected)
                return;
        }

        Debug.Log("Woooooooo Patern Complete");
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;

        lineRenderer.positionCount = cubes.Length;

        for (int i = 0; i < cubes.Length; i++)
        {
            lineRenderer.SetPosition(i, cubes[i].transform.position);
        }
    }
}

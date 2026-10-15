using UnityEngine;

public class RotateTesting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //first way
        // transform.Rotate(0, 50f * Time.deltaTime, 0);

        //second way
       // transform.rotation = Quaternion.Slerp(transform.rotation, 90, 5 * Time.deltaTime);
    }
}

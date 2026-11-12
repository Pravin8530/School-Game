using UnityEngine;

public class Fan : MonoBehaviour
{
    float rotationSpeed = 300f; // Adjust the speed as needed
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Vector3.forward * Time.deltaTime * 100f);
        transform.Rotate(0,0, rotationSpeed * Time.deltaTime,Space.Self);
    }
}

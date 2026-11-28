using UnityEngine;

public class FanController : MonoBehaviour
{

    [SerializeField] private float rotatingAngle = 90f;
    public float targetAngle;
    public float[] speed = { 300f, 600f, 900f, 1200f };
    public int speedIndex;
    public float targetSpeed;
    void Start()
    {
        FanSpeed(0);
    }

    void Update()
    {

    }

    public float GetAngle()
    {

        targetAngle += rotatingAngle;
        return targetAngle;
    }


    public float FanSpeed(int index)
    {

        targetSpeed = speed[index];
        Debug.Log("target speed is : " + targetSpeed);

        return targetSpeed;

    }

    public float GetSpeed()
    {
        return targetSpeed;

    }


}

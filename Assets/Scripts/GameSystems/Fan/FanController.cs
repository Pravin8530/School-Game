using UnityEngine;

public class FanController : MonoBehaviour
{

    [SerializeField] private float rotatingAngle = 90f;
    public float targetAngle;
    public float[] speed = { 300f, 600f, 900f, 1200f };
    public int speedIndex;
    public float targetSpeed;
    public GameObject rope;
    public bool isFacingRope;
    public GameObject fanBlades;
    void Start()
    {
        FanSpeed(0);
    }

    void Update()
    {
        GetFacingDirection();

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

    public bool GetFacingDirection()
    {
        Vector3 heading = rope.transform.position - fanBlades.transform.position;
        Vector3 dir = heading.normalized;

        float dot = Vector3.Dot(fanBlades.transform.forward, dir);

        if (dot > 0.7f)
        {
            Debug.Log("facing towards rope");
            isFacingRope = true;

        }
        else
        {
            isFacingRope = false;
        }

        return isFacingRope;
    }



}

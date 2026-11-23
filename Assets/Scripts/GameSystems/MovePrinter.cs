using NUnit.Framework;
using UnityEngine;

public class MovePrinter : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float shakeSpeed = 10f;     // High number = rapid motor rattle
    [SerializeField] private float shakeAmount = 0.01f;  // Tiny number = tight vibration
    [SerializeField] private float tiltAmount = 0.5f;    // Slight rocking angle in degrees

    private bool isPrinting;
    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Update()
    {
        if (isPrinting)
        {
            ShakePrinter();
        }
    }


    public void StartShaking()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        isPrinting = true;

    }

    public void StopShaking()
    {
        isPrinting = false;
        transform.position = startPos;
        transform.rotation = startRot;

    }









    public void ShakePrinter()
    {
        // 1. Rapid side-to-side rattle (high speed, tiny distance)
        float rattleX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;

        // 2. Slight up-and-down bounce at a slightly different rhythm so it feels erratic
        float rattleY = Mathf.Cos(Time.time * (shakeSpeed * 0f)) * (shakeAmount * 0.5f);

        transform.position = startPos + new Vector3(rattleX, rattleY, 0f);

        // 3. Tiny rotational rock on the Z-axis so the top wobbles
        float wobbleZ = Mathf.Sin(Time.time * (shakeSpeed * 0.5f)) * tiltAmount;
        transform.rotation = startRot * Quaternion.Euler(0f, 0f, wobbleZ);
    }
}

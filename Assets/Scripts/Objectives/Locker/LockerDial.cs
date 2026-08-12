using UnityEngine;

public class LockerDial : MonoBehaviour, IInteractables
{
    // Settings

    [Header("Dial Rotation")]
    [SerializeField] private float timeBetweenRotations = 1f;
    [SerializeField] private int rotationDirection = 1;

    [Header("Dial Numbers")]
    [SerializeField] private int totalNumbers = 10;
    [SerializeField] private int numberOffset = 0;
    [SerializeField] private bool reverseNumbers = false;


    // References 

    [SerializeField] private GameObject lockerDial;


    // State

    private float timer;
    private bool isRotating;

    public int currentNumber;
    public int Priority => 1;


    

    private void Update()
    {
        UpdateCurrentNumber();

        if (isRotating)
            RotateDial();
    }


    // Interaction 

    public void Interact()
    {
        Debug.Log("Dial Interacted");

        if (isRotating)
            StopDial();
        else
            StartDial();
    }


    private void StartDial()
    {
        isRotating = true;
    }


    private void StopDial()
    {
        isRotating = false;
        timer = 0f;
    }


    // Rotation

    private void RotateDial()
    {
        timer += Time.deltaTime;

        if (timer < timeBetweenRotations)
            return;

        float step = 360f / totalNumbers;

        lockerDial.transform.Rotate(
            0f,
            0f,
            step * rotationDirection
        );

        timer = 0f;
    }


    //  Number Calculation

    private void UpdateCurrentNumber()
    {
        float angle = lockerDial.transform.localEulerAngles.z;
        float step = 360f / totalNumbers;

        int number = Mathf.RoundToInt(angle / step) % totalNumbers;

        if (reverseNumbers)
            number = (totalNumbers - number) % totalNumbers;

        currentNumber = (number + numberOffset) % totalNumbers;
    }
}
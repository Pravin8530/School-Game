using UnityEngine;

public class DialController : MonoBehaviour
{
    [SerializeField] private int totalNumbers = 10;

    [SerializeField] private int numberOffset = 0;
    [SerializeField] private bool reverseNumbers = false;

    public int currentNumber;
    

    void Update()
    {
        float angle = transform.localEulerAngles.z;

        float step = 360f / totalNumbers;

        int number = Mathf.RoundToInt(angle / step) % totalNumbers; // formula mfs for converting 360 degrres into specific index.

        if (reverseNumbers)
            number = (totalNumbers - number) % totalNumbers;

        currentNumber = (number + numberOffset) % totalNumbers;
    }
}
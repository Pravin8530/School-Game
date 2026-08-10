using UnityEngine;
using TMPro;

public class LockerPassward : MonoBehaviour
{
    public int[] correctPassward = { 1, 2, 3, 4 };
     
     public int[] enteredNumbers;
     public TextMeshProUGUI[] numberDisplays;
     public GameObject Lever;
     public Lever lever;
  
    public DialController dialController;

    public LockerDial lockerDial;

    public void Awake()
    { 
        enteredNumbers = new int[4];
        dialController = GetComponent<DialController>();
        lockerDial = GetComponent<LockerDial>();
        lever = Lever. GetComponent<Lever>();
        if(lever == null)
        {
            Debug.LogError("Lever component not found on the GameObject.");
        }
        lever.currentDigit = 0;
       GenrateRandomPassward();
      // Debug.Log("Correct Passward: " + string.Join(", ", correctPassward));

    }
 
    // Update is called once per frame
    void Update()
    {
              //  CheckPassward();
    
                   HandleEnteredNumber(dialController.currentNumber, lever.currentDigit);

    }
  
     
     private void HandleEnteredNumber(int number, int digitIndex)
     {
      
         numberDisplays[digitIndex].text = number.ToString();
         enteredNumbers[digitIndex] = number;
            
     }

    
    private void GenrateRandomPassward()
    {
        for (int i = 0; i < correctPassward.Length; i++)
        {
            correctPassward[i] = Random.Range(0, 10);
        }
    }


    private void CheckPassward()
    {
        bool isCorrect = true;

        for (int i = 0; i < correctPassward.Length; i++)
        {
            if (enteredNumbers[i] != correctPassward[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("Passward is correct!");
        }
        else
        {
            Debug.Log("Passward is incorrect!");
        }
    }
  

}

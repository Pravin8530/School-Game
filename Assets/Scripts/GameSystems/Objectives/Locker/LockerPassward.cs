using UnityEngine;
using TMPro;
using System.Collections;

public class LockerPassward : MonoBehaviour
{
    public int[] correctPassward = { 1, 2, 3, 4 };
     
     public int[] enteredNumbers;
     public TextMeshProUGUI[] numberDisplays;
     public GameObject Lever;
     public Lever lever;
  

    public LockerDial lockerDial;
  
     public GameObject DoorPivot;
        bool isSolved = false;
    public void Awake()
    { 
        enteredNumbers = new int[4];
        lockerDial = GetComponent<LockerDial>();
        lever = Lever. GetComponent<Lever>();
        if(lever == null)
        {
            Debug.LogError("Lever component not found on the GameObject.");
        }
        lever.currentDigit = 0;
       GenrateRandomPassward();

    }
 
    void Update()
    { 
          if(!isSolved){
                CheckPassward();

          } 
                   HandleEnteredNumber(lockerDial.currentNumber, lever.currentDigit);

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
        Debug.Log("Generated Passward: " + string.Join(", ", correctPassward));
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
            isSolved = true;
            if(lockerDial != null)
            {
                lockerDial.StopDial();
            }
            StartCoroutine(RotateDoor());
        }
        else
        {
            Debug.Log("Passward is incorrect!");
        }
    }
  
   IEnumerator RotateDoor()
    {
        float rotationSpeed = 100f;
        float rotationAmount = 70f; 

        Quaternion initialRotation = DoorPivot.transform.localRotation;
        Quaternion targetRotation =  Quaternion.Euler(0f, 90f, 0f)* initialRotation;
        float duration = rotationAmount / rotationSpeed;
        
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {  
             timeElapsed += Time.deltaTime;
          float   t = timeElapsed / duration;
           DoorPivot.transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, t);  
                      yield return null;

        }
       
       
    }

}

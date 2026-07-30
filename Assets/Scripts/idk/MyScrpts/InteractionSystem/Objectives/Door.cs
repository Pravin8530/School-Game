using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    bool isOpen;
   [SerializeField] public Transform doorPivot;    
 
  [SerializeField] private float openAngle;
  [SerializeField] private float closeAngle;
  [SerializeField] private float openSpeed = 10f;

  [SerializeField] private bool isDoorLocked;   
  [SerializeField] private float lockedDoorAnimationSpeed = 1f;
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    { 
      float targetY = isOpen ? openAngle : closeAngle;

        doorPivot.localRotation = Quaternion.Slerp(
            doorPivot.localRotation,
            Quaternion.Euler(0, targetY, 0),
            Time.deltaTime * openSpeed  );  
    }

    public void Interact()
    {         
       if(isDoorLocked) return;    
        isOpen = !isOpen;
    }
    public void Drop()
    {
        isOpen = false;
    }
}

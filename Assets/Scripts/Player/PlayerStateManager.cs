using UnityEngine;
using UnityEngine.Purchasing;

public class PlayerStateManager : MonoBehaviour
{ 
    public static PlayerStateManager Instance;

   
   
    public PlayerState currentState { get; private set; }

    public Paper HeldPaper { get; private set; }

    public enum PlayerState
    {
        Normal,
        Inspecting,
        Crouching,
        Hiding,
        BookInteract,
        HasImportantItem, 
        HasPaper,
        HoldingItem
   
    }

    
    void Awake()
    { 
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;

        }
        Instance = this;
        currentState = PlayerState.Normal;
       
      DontDestroyOnLoad(gameObject);
    }


    //public void SetState(PlayerState newState)
    //{
    //    currentState = newState;

    //}

    public void SetState(PlayerState newState, Paper PaperItem = null )
    {

        // FIX 1: Set the state to the NEW state coming in, not hardcoded to Normal!
        currentState = newState;

        if (newState == PlayerState.HasPaper)
        {
            // FIX 2: Store the paper script that was picked up into our tracking variable
            HeldPaper = PaperItem;
        }
        else
        {
            if (HeldPaper != null)
            {
                // Unparent it from the camera if they switched states without printing
                HeldPaper.transform.SetParent(null);

                // Re-enable physics so it falls to the ground naturally
                Rigidbody rb = HeldPaper.GetComponent<Rigidbody>();
                Collider col = HeldPaper.GetComponent<Collider>();
                if (rb != null) rb.useGravity = true;
                if (col != null) col.enabled = true;
            }

            //clearing the ref
            HeldPaper = null;
        }

    }

    
}

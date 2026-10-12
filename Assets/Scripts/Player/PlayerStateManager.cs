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


  

    public void SetState(PlayerState newState, Paper PaperItem = null )
    {

        currentState = newState;

        if (newState == PlayerState.HasPaper)
        {
            HeldPaper = PaperItem;
        }
        else
        {
            if (HeldPaper != null)
            {
                HeldPaper.transform.SetParent(null);

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

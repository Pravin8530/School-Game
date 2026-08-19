using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{ 
    public static PlayerStateManager Instance;

   
    public PlayerState currentState { get; private set; }

    public enum PlayerState
    {
        Normal,
        Inspecting,
        Crouching,
        Hiding,
        BookInteract
        

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


    public void SetState(PlayerState newState)
    {
        currentState = newState;

    }


}

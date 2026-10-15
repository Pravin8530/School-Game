using TMPro;
using UnityEngine;

public class TeacherDebugUi : MonoBehaviour
{
    TeacherController teacher;
    private ITeacherState currentState;

    [Header("Debugging_Ui")]
    public TextMeshProUGUI currentAiState;
    public TextMeshProUGUI canSeeText;
    public TextMeshProUGUI HeardSoundText;
    public TextMeshProUGUI stunedText;

    [SerializeField] public bool hasHeardSound = false;
    [SerializeField] public bool isStunned = false; 
    
    void Start()
    {
        teacher = GetComponent<TeacherController>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Update();
        
        //debug txt thingy
        if (currentAiState != null)
        {
            currentAiState.text = "Current Ai State: " + (currentState?.GetType().Name ?? "None");
        }
        if (canSeeText != null)
        {
            bool canSee = teacher.CanSeePlayer();
            canSeeText.text = "Can See Player: " + canSee;
            canSeeText.color = canSee ? Color.green : Color.red;
        }
        if (HeardSoundText != null)
        {
            HeardSoundText.text = "Heard Sound: " + hasHeardSound;
            HeardSoundText.color = hasHeardSound ? Color.green : Color.red;
        }
        if (stunedText != null)
        {
            stunedText.text = "Is Stunned: " + isStunned;
            stunedText.color = isStunned ? Color.green : Color.red;
        }

    }
}

using TMPro;
using UnityEngine;

public class TeacherDebugUi : MonoBehaviour
{
    TeacherController teacher;


    [Header("Debugging_Ui")]
    public TextMeshProUGUI currentAiState;
    public TextMeshProUGUI canSeeText;
    public TextMeshProUGUI HeardSoundText;
    public TextMeshProUGUI stunedText;

    public TextMeshProUGUI nationalAnthamCd;

    void Start()
    {
        teacher = GetComponent<TeacherController>();
    }

    void Update()
    {
        ITeacherState currentState = teacher.CurrentState;

        currentState?.Update();
        
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
            HeardSoundText.text = "Heard Sound: " + teacher.hasHeardSound;
            HeardSoundText.color = teacher.hasHeardSound ? Color.green : Color.red;
        }
        if (stunedText != null)
        {
            stunedText.text = "Is Stunned: " +teacher.isStunned;
            stunedText.color = teacher.isStunned ? Color.green : Color.red;
        }
        if(nationalAnthamCd != null)
        {
            nationalAnthamCd.text = "nationalAntham: " + teacher.nationalAnthamDuriation;


        }

    }
}

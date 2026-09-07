using UnityEngine;
using UnityEngine.AI;

public class StunState : ITeacherState
{
    private TeacherController teacher;
    private NavMeshAgent agent;
    private float stunDuration;
    private float timer;
 
    //debug Ui
    private bool isStunned = true;

    
   
    public StunState(TeacherController teacher, float duration)
    {
        this.teacher = teacher;
        this.stunDuration = duration;
        agent = teacher.GetComponent<NavMeshAgent>();
    }

    public void Enter()
    {
        Debug.Log("Teacher is stunned!");

        // Stop the agent completely
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        timer = 0f;

        // Play stun animation if you have one
        // teacher.GetComponent<Animator>()?.SetTrigger("Stunned");
        teacher.isStunned = true;
        teacher.stunedText.text = "Stunned: " + teacher.isStunned;

        
    }

    public void Update()
    {

        timer += Time.deltaTime;

        // Wait out the stun duration
        if (timer >= stunDuration)
        {
            // Set search target to where the player was last seen
            teacher.investigationPoint = teacher.lastKnownPosition;
            
            // Resume agent and transition to SearchState
            agent.isStopped = false;
            teacher.ChangeState(new SearchState(teacher));
        }
      
    }

    public void Exit()
    {
        agent.isStopped = false;
        
        // teacher.isStunned = false;
        // teacher.stunedText.text = "Stunned: " + teacher.isStunned;
    }
}
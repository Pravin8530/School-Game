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
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        timer = 0f;

        //  stun animation 
        teacher.PlayAnimation(teacher.stunClip, 0.15f);
        teacher.isStunned = true;
        teacher.stunedText.text = "Stunned: " + teacher.isStunned;


    }

    public void Update()
    {

        timer += Time.deltaTime;

        if (timer >= stunDuration)
        {
            teacher.isStunned = false;
            teacher.stunedText.text = "Stunned: " + teacher.isStunned;
            teacher.investigationPoint = teacher.lastKnownPosition;
            agent.isStopped = false;
            teacher.ChangeState(new SearchState(teacher));
        }

    }

    public void Exit()
    {
        agent.isStopped = false;


    }
}
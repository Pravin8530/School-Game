using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SearchState : ITeacherState
{
    private TeacherController teacher;
    private float stateTimer = 0f;

    private Quaternion startRotation;
    private Quaternion leftRotation;
    private Quaternion rightRotation;

    private NavMeshAgent agent;
    private bool reachedDestination = false;
    private bool playedSearchAnim = false; 

    public SearchState(TeacherController teacher)
    {
        this.teacher = teacher;
        this.agent = teacher.GetComponent<NavMeshAgent>();
        
    }

  
    public void Enter()
    {
         
        Debug.Log("Investigating sound location");
        stateTimer = 0f;
        reachedDestination = false;
        playedSearchAnim = false;

        agent.isStopped = false;
        agent.stoppingDistance = 0.8f; 
        agent.SetDestination(teacher.investigationPoint);

        teacher.teacherAnimation.PlayAnimation(teacher.teacherAnimation.patrolClip, 0.15f);

        teacher.hasHeardSound = true;
        if (teacher.teacherDebug.HeardSoundText != null)
            teacher.teacherDebug.HeardSoundText.text = "Heard Sound: " + teacher.hasHeardSound;
    }

    public void Update()
    {
        
        if (teacher.CanSeePlayer())
        {
            teacher.ChangeState(new ChaseState(teacher));
            return;
        }

        if (!reachedDestination)
        {
            if (!agent.pathPending && agent.hasPath)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    reachedDestination = true;
                    agent.isStopped = true;
                    agent.ResetPath(); 

                    startRotation = teacher.transform.rotation;
                    leftRotation = startRotation * Quaternion.Euler(0, -60f, 0);
                    rightRotation = startRotation * Quaternion.Euler(0, 60f, 0);
                }
            }
            return; 
        }

        if (!playedSearchAnim)
        {
            Debug.Log("Reached sound location. Playing Search Animation!");
           teacher.teacherAnimation.PlayAnimation(teacher.teacherAnimation.searchClip, 0.15f);
            playedSearchAnim = true;
        }

        stateTimer += Time.deltaTime;

        if (stateTimer < 1.5f)
        {
            teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, leftRotation, Time.deltaTime * 3f);
        }
        else if (stateTimer < 3.5f)
        {
            teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, rightRotation, Time.deltaTime * 3f);
        }
        else if (stateTimer < 5.0f)
        {
            teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, startRotation, Time.deltaTime * 3f);
        }
        else
        {
            teacher.ChangeState(new PetrolState(teacher));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Search State");

        teacher.hasHeardSound = false;
        if (teacher.teacherDebug.HeardSoundText != null)
            teacher.teacherDebug.HeardSoundText.text = "Heard Sound: " + teacher.hasHeardSound;

        teacher.isStunned = false;
        if (teacher.teacherDebug.stunedText != null)
            teacher.teacherDebug.stunedText.text = "Stunned: " + teacher.isStunned;
    }
}
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : ITeacherState
{
    private TeacherController teacher;
    private NavMeshAgent agent;

    private float timer = 0f;
    private float delay = 0f;
    private bool isDoneWaiting = false;

    public ChaseState(TeacherController teacher)
    {
        this.teacher = teacher;
        agent = teacher.GetComponent<NavMeshAgent>();
    }

    public void Enter()
    {
        //playing animation before chasing.


        // Stop moving while reacting
        agent.isStopped = true;
        timer = 0f;
        isDoneWaiting = false;

        // Check if player was seen recently
        if (Time.time - teacher.lastSeenTimer < teacher.memoryTime)
        {
            delay = teacher.quickNoticeTime; // 0.2 seconds
        }
        else
        {
            delay = teacher.firstNoticeTime; // 2.0 seconds
        }

        teacher.lastSeenTimer = Time.time;
    }

   
    public void Update()
    {
        // Step 1: Wait for reaction delay
        if (!isDoneWaiting)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                isDoneWaiting = true;
                agent.isStopped = false;
            }
            return;
        }

        // Catch check (works 360 degrees if touched)
        float distanceToPlayer = Vector3.Distance(teacher.transform.position, teacher.player.transform.position);
        if (distanceToPlayer <= 2.5f)
        {
            teacher.ChangeState(new CatchState(teacher));
            return;
        }

        // Step 2: Vision check
        if (teacher.CanSeePlayer())
        {
            teacher.lastSeenTimer = Time.time;
            teacher.lastKnownPosition = teacher.player.transform.position; // Update last known position
            agent.SetDestination(teacher.player.transform.position);
        }
        else
        {
            
            teacher.investigationPoint = teacher.lastKnownPosition;
            teacher.ChangeState(new SearchState(teacher));
        }
    }

    public void Exit()
    {
        agent.isStopped = false;
    }
}
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
        Debug.Log("Spotted Player!");

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

    // public void Update()
    // {
    //     // Step 1: Wait for reaction delay
    //     if (!isDoneWaiting)
    //     {
    //         timer += Time.deltaTime;
    //         if (timer >= delay)
    //         {
    //             isDoneWaiting = true;
    //             agent.isStopped = false; // Start moving again
    //         }
    //         return; // Don't move until timer finishes
    //     }

    //     // Step 2: Chase or Go to Last Known Position
    //     if (teacher.CanSeePlayer())
    //     {
    //         float distanceToPlayer = Vector3.Distance(teacher.transform.position, teacher.player.transform.position);

    //         // If teacher gets very close to player (e.g. within 1.5 units), trigger catch
    //         if (distanceToPlayer <= 2.5f)
    //         {
    //             teacher.ChangeState(new CatchState(teacher));
    //             return;
    //         }


    //         teacher.lastSeenTimer = Time.time;
    //         agent.SetDestination(teacher.player.transform.position);
    //     }
    //     else
    //     {
    //         // Walk to last known position
    //         agent.SetDestination(teacher.lastKnownPosition);

    //         // Reached last position? Go to Search state
    //         if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
    //         {
    //             teacher.ChangeState(new SearchState(teacher));
    //         }
    //     }
    // }

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
            agent.SetDestination(teacher.lastKnownPosition);

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                teacher.ChangeState(new SearchState(teacher));
            }
        }
    }

    public void Exit()
    {
        agent.isStopped = false;
    }
}
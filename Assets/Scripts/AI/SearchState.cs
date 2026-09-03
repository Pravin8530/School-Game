using UnityEngine;
using UnityEngine.AI;
public class SearchState : ITeacherState
{
    private TeacherController teacher;
    private float stateTimer = 0f;

    // Angles relative to the direction the teacher was facing when entering the state
    private Quaternion startRotation;
    private Quaternion leftRotation;
    private Quaternion rightRotation;

    private NavMeshAgent agent;
    private bool reachedDestination = false;

    // public SearchState(TeacherController teacher)
    // {
    //     this.teacher = teacher;
    // }

    // public void Enter()
    // {
    //     Debug.Log("Searching last known area...");
    //     stateTimer = 0f;

    //     // Calculate rotation points (look 60 degrees left and right)
    //     startRotation = teacher.transform.rotation;
    //     leftRotation = startRotation * Quaternion.Euler(0, -60f, 0);
    //     rightRotation = startRotation * Quaternion.Euler(0, 60f, 0);
    // }

    public SearchState(TeacherController teacher)
    {
        this.teacher = teacher;
        this.agent = teacher.GetComponent<NavMeshAgent>(); // ADD THIS
    }

    // public void Enter()
    // {
    //     Debug.Log("Investigating sound location...");
    //     stateTimer = 0f;
    //     reachedDestination = false; // ADD THIS

    //     agent.isStopped = false; // ADD THIS
    //     agent.SetDestination(teacher.soundLocation); // ADD THIS: Walk to noise location!
    // }

    // // public void Update()
    // // {
    // //     // 1. Interrupt if player is spotted
    // //     if (teacher.CanSeePlayer())
    // //     {
    // //         teacher.ChangeState(new ChaseState(teacher));
    // //         return;
    // //     }

    // //     stateTimer += Time.deltaTime;

    // //     // 2. Step-by-step search sequence (5 seconds total)
    // //     if (stateTimer < 1.5f)
    // //     {
    // //         // Look Left
    // //         teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, leftRotation, Time.deltaTime * 3f);
    // //     }
    // //     else if (stateTimer < 3.5f)
    // //     {
    // //         // Look Right
    // //         teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, rightRotation, Time.deltaTime * 3f);
    // //     }
    // //     else if (stateTimer < 5.0f)
    // //     {
    // //         // Look back Center
    // //         teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, startRotation, Time.deltaTime * 3f);
    // //     }
    // //     else
    // //     {
    // //         // Finish search and resume patrol
    // //         Debug.Log("Area clear. Resuming patrol.");
    // //         teacher.ChangeState(new PetrolState(teacher));
    // //     }
    // // }

    // public void Update()
    // {
    //     // 1. Interrupt if player is spotted
    //     if (teacher.CanSeePlayer())
    //     {
    //         teacher.ChangeState(new ChaseState(teacher));
    //         return;
    //     }

    //     // 2. Walk to sound location first
    //     if (!reachedDestination)
    //     {
    //         // Wait for path to calculate and check if teacher reached the sound spot
    //         if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
    //         {
    //             reachedDestination = true;
    //             agent.isStopped = true; // Stop moving to look around

    //             // Calculate rotation points relative to arrival facing direction
    //             startRotation = teacher.transform.rotation;
    //             leftRotation = startRotation * Quaternion.Euler(0, -60f, 0);
    //             rightRotation = startRotation * Quaternion.Euler(0, 60f, 0);
    //         }
    //         return; // Don't start look-around timer until teacher arrives!
    //     }

    //     // 3. Look-around sequence (starts ONLY after arriving at the sound spot)
    //     stateTimer += Time.deltaTime;

    //     if (stateTimer < 1.5f)
    //     {
    //         teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, leftRotation, Time.deltaTime * 3f);
    //     }
    //     else if (stateTimer < 3.5f)
    //     {
    //         teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, rightRotation, Time.deltaTime * 3f);
    //     }
    //     else if (stateTimer < 5.0f)
    //     {
    //         teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, startRotation, Time.deltaTime * 3f);
    //     }
    //     else
    //     {
    //         Debug.Log("Area clear. Resuming patrol.");
    //         teacher.ChangeState(new PetrolState(teacher));
    //     }
    // }

    public void Enter()
    {
        Debug.Log("Investigating sound location...");
        stateTimer = 0f;
        reachedDestination = false;

        // 1. Reset stopping state completely
        agent.isStopped = false;
        agent.stoppingDistance = 0.5f; // Keep this low so teacher gets close to noise
        agent.SetDestination(teacher.investigationPoint); // Walk to noise location
    }

    public void Update()
    {
        // Interrupt if player is spotted visually
        if (teacher.CanSeePlayer())
        {
            teacher.ChangeState(new ChaseState(teacher));
            return;
        }

        // Step A: Walk to sound location
        if (!reachedDestination)
        {
            // IMPORTANT: Must check (agent.pathPending == false) AND (hasPath) 
            // to prevent frame-1 false arrival triggers!
            if (!agent.pathPending && agent.hasPath)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    reachedDestination = true;
                    agent.isStopped = true; // Stop walking to begin looking around

                    // Lock rotations at arrival spot
                    startRotation = teacher.transform.rotation;
                    leftRotation = startRotation * Quaternion.Euler(0, -60f, 0);
                    rightRotation = startRotation * Quaternion.Euler(0, 60f, 0);
                }
            }
            return;
        }

        // Step B: Look around sequence
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
            Debug.Log("Area clear. Resuming patrol.");
            teacher.ChangeState(new PetrolState(teacher));
        }
    }



    public void Exit()
    {
        Debug.Log("Exiting Search State");
    }
}
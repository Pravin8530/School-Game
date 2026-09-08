// using UnityEngine;
// using UnityEngine.AI;
// public class SearchState : ITeacherState
// {
//     private TeacherController teacher;
//     private float stateTimer = 0f;

//     // Angles relative to the direction the teacher was facing when entering the state
//     private Quaternion startRotation;
//     private Quaternion leftRotation;
//     private Quaternion rightRotation;

//     private NavMeshAgent agent;
//     private bool reachedDestination = false;
//     // ADD THIS LINE under 'private bool reachedDestination = false;'
//     public SearchState(TeacherController teacher)
//     {
//         this.teacher = teacher;
//         this.agent = teacher.GetComponent<NavMeshAgent>(); // ADD THIS
//     }

//     public void Enter()
//     {

//         Debug.Log("Investigating sound location...");
//         stateTimer = 0f;
//         reachedDestination = false;

//         // 1. Reset stopping state completely
//         agent.isStopped = false;
//         agent.stoppingDistance = 0.5f; // Keep this low so teacher gets close to noise
//         agent.SetDestination(teacher.investigationPoint); // Walk to noise location

//         teacher.hasHeardSound = true; // Reset the sound flag when entering the state
//         teacher.HeardSoundText.text = "Heard Sound: " + teacher.hasHeardSound; // Update UI for sound state


//     }

//     public void Update()
//     {
//         // Interrupt if player is spotted visually
//         if (teacher.CanSeePlayer())
//         {
//             teacher.ChangeState(new ChaseState(teacher));
//             return;
//         }

//         // Step A: Walk to sound location
//         if (!reachedDestination)
//         {
            
//             if (!agent.pathPending && agent.hasPath)
//             {
//                 if (agent.remainingDistance <= agent.stoppingDistance)
//                 {

//                     reachedDestination = true;
//                     agent.isStopped = true; // Stop walking to begin looking around

//                     teacher.PlayAnimation(teacher.searchClip, 0.15f);

//                     // Lock rotations at arrival spot
//                     startRotation = teacher.transform.rotation;
//                     leftRotation = startRotation * Quaternion.Euler(0, -60f, 0);
//                     rightRotation = startRotation * Quaternion.Euler(0, 60f, 0);
//                 }
//             }
//             return;
//         }


//         // Step B: Look around sequence
//         stateTimer += Time.deltaTime;

//         if (stateTimer < 1.5f)
//         {
//             teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, leftRotation, Time.deltaTime * 3f);
//         }
//         else if (stateTimer < 3.5f)
//         {
//             teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, rightRotation, Time.deltaTime * 3f);
//         }
//         else if (stateTimer < 5.0f)
//         {
//             teacher.transform.rotation = Quaternion.Slerp(teacher.transform.rotation, startRotation, Time.deltaTime * 3f);
//         }
//         else
//         {
//             Debug.Log("Area clear. Resuming patrol.");
//             teacher.ChangeState(new PetrolState(teacher));
//         }
//     }



//     public void Exit()
//     {
//         Debug.Log("Exiting Search State");

//         teacher.hasHeardSound = false; // Reset the sound flag when leaving the state
//         teacher.HeardSoundText.text = "Heard Sound: " + teacher.hasHeardSound; // Update UI for sound state
//         teacher.isStunned = false;
//         teacher.stunedText.text = "Stunned: " + teacher.isStunned;

//     }
// } 

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
    private bool playedSearchAnim = false; // Prevents calling PlayAnimation every frame

    public SearchState(TeacherController teacher)
    {
        this.teacher = teacher;
        this.agent = teacher.GetComponent<NavMeshAgent>();
    }

    public void Enter()
    {
        Debug.Log("Investigating sound location...");
        stateTimer = 0f;
        reachedDestination = false;
        playedSearchAnim = false;

        agent.isStopped = false;
        agent.stoppingDistance = 0.8f; // Slightly increased for NavMesh stability
        agent.SetDestination(teacher.investigationPoint);

        // Explicitly play walking animation while traveling to the sound
        teacher.PlayAnimation(teacher.patrolClip, 0.15f);

        teacher.hasHeardSound = true;
        if (teacher.HeardSoundText != null)
            teacher.HeardSoundText.text = "Heard Sound: " + teacher.hasHeardSound;
    }

    public void Update()
    {
        // 1. Vision priority
        if (teacher.CanSeePlayer())
        {
            teacher.ChangeState(new ChaseState(teacher));
            return;
        }

        // 2. Navigation Phase
        if (!reachedDestination)
        {
            if (!agent.pathPending && agent.hasPath)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    reachedDestination = true;
                    agent.isStopped = true;
                    agent.ResetPath(); // Stop the agent completely

                    // Lock rotations at arrival spot
                    startRotation = teacher.transform.rotation;
                    leftRotation = startRotation * Quaternion.Euler(0, -60f, 0);
                    rightRotation = startRotation * Quaternion.Euler(0, 60f, 0);
                }
            }
            return; // Wait until arrival
        }

        if (!playedSearchAnim)
        {
            Debug.Log("Reached sound location. Playing Search Animation!");
            teacher.PlayAnimation(teacher.searchClip, 0.15f);
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
            Debug.Log("Area clear. Resuming patrol.");
            teacher.ChangeState(new PetrolState(teacher));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Search State");

        teacher.hasHeardSound = false;
        if (teacher.HeardSoundText != null)
            teacher.HeardSoundText.text = "Heard Sound: " + teacher.hasHeardSound;

        teacher.isStunned = false;
        if (teacher.stunedText != null)
            teacher.stunedText.text = "Stunned: " + teacher.isStunned;
    }
}
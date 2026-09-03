using UnityEngine;
using UnityEngine.AI;

public class CatchState : ITeacherState
{
    private TeacherController teacher;
    private NavMeshAgent agent;

    private float timer = 0f;
    private float catchAnimationTime = 5.0f; // Duration of the catch animation in seconds
    private bool playerHidden = false;

    public CatchState(TeacherController teacher)
    {
        this.teacher = teacher;
        agent = teacher.GetComponent<NavMeshAgent>();
    }

    public void Enter()
    {
        Debug.Log("Player Caught!");

        // 1. Freeze movement completely
        agent.isStopped = true;
        agent.ResetPath();

        timer = 0f;
        playerHidden = false;

        // 2. Play catch animation (if using Animator)
        // teacher.GetComponent<Animator>()?.SetTrigger("CatchPlayer");
    }

    public void Update()
    {
        // 3. Smoothly rotate toward the player during the animation
        if (teacher.player != null)
        {
            Vector3 direction = (teacher.player.transform.position - teacher.transform.position).normalized;
            direction.y = 0; // Prevent tilting up or down

            if (direction != Vector3.zero)
            {
                teacher.transform.rotation = Quaternion.Slerp(
                    teacher.transform.rotation,
                    Quaternion.LookRotation(direction),
                    Time.deltaTime * 10f
                );
            }
        }

        // 4. Count down animation time
        timer += Time.deltaTime;

        // Hide player right when timer finishes
        if (timer >= catchAnimationTime && !playerHidden)
        {
            playerHidden = true;

            if (teacher.player != null)
            {
                teacher.player.SetActive(false); // Disappears the player
                Debug.Log("Player has disappeared. Game Over / Reset state.");
            }

            // Return to patrol or handle Game Over UI
            //GAME OVER HAHAHAH
            teacher.ChangeState(new PetrolState(teacher));
        }
    }

    public void Exit()
    {
        agent.isStopped = false;
    }
}
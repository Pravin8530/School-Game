using UnityEngine;
using UnityEngine.AI;

public class CatchState : ITeacherState
{
    private TeacherController teacher;
    private NavMeshAgent agent;

    private float timer = 0f;
    private float catchAnimationTime = 1.0f; 
    private bool playerHidden = false;

    public CatchState(TeacherController teacher)
    {
        this.teacher = teacher;
        agent = teacher.GetComponent<NavMeshAgent>();
    }

    public void Enter()
    {
        Debug.Log("Die Motherfucker!");
        agent.isStopped = true;
        agent.ResetPath();

        timer = 0f;
        playerHidden = false;

        teacher.PlayAnimation(teacher.catchClip, 0.15f);
    }

    public void Update()
    {
     

        timer += Time.deltaTime;

        if (timer >= catchAnimationTime && !playerHidden)
        {
            playerHidden = true;

            if (teacher.player != null)
            {
                teacher.player.SetActive(false); 
                Debug.Log("Player has disappeared. Game Over / Reset state.");
            }

            teacher.ChangeState(new PetrolState(teacher));
        }
    }

    public void Exit()
    {
        agent.isStopped = false;
    }
}
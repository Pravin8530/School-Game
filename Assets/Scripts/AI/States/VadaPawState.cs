using UnityEngine;
using UnityEngine.AI;

public class VadaPawState : ITeacherState
{
    private TeacherController teacher;
    private NavMeshAgent agent;
    private Transform vadaPawPosition;
    private GameObject vadaPawPrefab;

    private float timer = 0f;
    private bool isEating = false;

    public VadaPawState(TeacherController teacher, Transform vadaPawPosition, GameObject vadapawPrefab = null)
    {
        this.teacher = teacher;
        this.vadaPawPosition = vadaPawPosition;
        this.vadaPawPrefab = vadapawPrefab;
        agent = teacher.GetComponent<NavMeshAgent>();
    }

    public void Enter()
    {
        isEating = false;
        timer = 0f;

        Debug.Log("Entered eating mode");
        if (vadaPawPosition != null)
        {
            agent.SetDestination(vadaPawPosition.position);
        }
        teacher.teacherAnimation.PlayAnimation(teacher.teacherAnimation.chaseClip, 0.15f);
    }

    public void Update()
    {
        if (isEating)
        {
            timer += Time.deltaTime;
            if (timer >= teacher.EatingTimer)
            {
                teacher.ChangeState(new PetrolState(teacher));
            }
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 2f)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.05f)
            {
                teacher.teacherAnimation.PlayAnimation(teacher.teacherAnimation.saluteClip, 0.15f);
                agent.ResetPath();
                isEating = true;
                vadaPawPrefab.GetComponent<VadaPawPrefab>().DestroyVadaPawPrefab();

            }
        }
    }

    public void Exit()
    {
        Debug.Log("Exited VadaPaw eating state");
    }
}

using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class VadaPawState : ITeacherState
{
    TeacherController teacher;
    NavMeshAgent agent;

    Transform vadaPawPosition;



    private float timer = 0f;
    bool isEating;
    public  VadaPawState(TeacherController teacher, Transform vadaPawPosition)
    {
        this.teacher = teacher;
        this.vadaPawPosition = vadaPawPosition;

        agent = teacher.GetComponent<NavMeshAgent>();

    }
    public void Enter()
    {
        isEating = false;

        Debug.Log("entered eating mode");
        agent.SetDestination(vadaPawPosition.position);
        teacher.teacherAnimation.PlayAnimation(teacher.teacherAnimation.chaseClip, 0.15f);
       
    }

    public void Update()
    {
        if (isEating) 
        {
           teacher.teacherAudio.PlaySoundClip();
            timer += Time.deltaTime;
            if (timer >= teacher.nationalAnthamDuriation)
            {
                teacher.ChangeState(new PetrolState(teacher));

            }
                return;


          }


            if (!agent.pathPending) {

            if (agent.remainingDistance <= agent.stoppingDistance + 2f)
            {

                if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.05f)
                {
                    teacher.teacherAnimation.PlayAnimation(teacher.teacherAnimation.saluteClip, 0.15f);

                    agent.ResetPath();
                    isEating = true;

                }
            }

            
        }


    }

    public void Exit()
    { 

        Debug.Log("exit the patriotic  state");

    }


   
}

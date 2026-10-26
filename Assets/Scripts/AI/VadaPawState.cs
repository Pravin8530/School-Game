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
    bool isSaluted;
    public  VadaPawState(TeacherController teacher, Transform vadaPawPosition)
    {
        this.teacher = teacher;
        this.vadaPawPosition = vadaPawPosition;

        agent = teacher.GetComponent<NavMeshAgent>();

    }
    public void Enter()
    {
        isSaluted = false;

        Debug.Log("entered the patriotic mode");
        agent.SetDestination(vadaPawPosition.position);
        teacher.teacherAnimation.PlayAnimation(teacher.teacherAnimation.chaseClip, 0.15f);
       
    }

    public void Update()
    {
        if (isSaluted) 
        {
          //  teacher.teacherAudio.PlaySoundClip(teacher.teacherAudio.nationalAnthem);
           teacher.teacherAudio.PlaySoundClip();
            timer += Time.deltaTime;
           // teacher.LookAtPlayer();
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
                    isSaluted = true;

                }
            }

            
        }


    }

    public void Exit()
    { 

        Debug.Log("exit the patriotic  state");

    }


   
}

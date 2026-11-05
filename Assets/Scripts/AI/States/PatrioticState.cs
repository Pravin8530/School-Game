using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class PatrioticState : ITeacherState
{
    TeacherController teacher;
    NavMeshAgent agent;

    Transform salutePosition;



    private float timer = 0f;
    bool isSaluted;
    public  PatrioticState(TeacherController teacher, Transform salutePosition)
    {
        this.teacher = teacher;
        this.salutePosition = salutePosition;

        agent = teacher.GetComponent<NavMeshAgent>();

    }
    public void Enter()
    {
        isSaluted = false;

        Debug.Log("entered the patriotic mode");
        agent.SetDestination(salutePosition.position);
        teacher.teacherAnimation.PlayAnimation(teacher.teacherAnimation.chaseClip, 0.15f);
       
    }

    public void Update()
    {
        if (isSaluted) 
        {
            AudioManager.Instance.PlayAudioClip(teacher.AudioData.audioClips,0);
           
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
          // typecasting Examples
        // int gameIndex = 5;
        // float castValue;

        // castValue = (float)gameIndex;

        Debug.Log("exit the patriotic  state");

    }


   
}

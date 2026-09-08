using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetrolState : ITeacherState
{


    private TeacherController teacher;
    private Transform currentTarget;
    private float speed = 5f;

    private NavMeshAgent teacherAI;

    private int currentPetrolIndex = 0;


    public PetrolState(TeacherController teacher)
    {

        this.teacher = teacher;
        teacherAI = teacher.GetComponent<NavMeshAgent>();

    }



    public void Enter()
    {
        Debug.Log("entering Petrol State");
         teacherAI.isStopped = false; 
        SetNextTarget();

        teacher.PlayAnimation(teacher.patrolClip,0.15f);
    }

    public void Update()
    {

        if (teacher.CanSeePlayer())
        {
            Debug.Log(teacher.CanSeePlayer());
            teacher.ChangeState(new ChaseState(teacher));

        }

        MoveToNextPoint();

    }

    public void Exit()
    {
        Debug.Log("Exioting Petrol");
        teacherAI.ResetPath();
        
    }


    private void MoveToNextPoint()
    {

        // null safegaurd
        if (teacher.petrolPoints == null && teacher.petrolPoints.Count == 0)
        {
            Debug.Log("No petrol points set");
            return;
        }

        if (!teacherAI.pathPending && teacherAI.remainingDistance <= teacherAI.stoppingDistance)
        {
            currentPetrolIndex++;

            if (currentPetrolIndex >= teacher.petrolPoints.Count)
            {

                currentPetrolIndex = 0;
            }
        
          /// based on chances like bara
            SetNextTarget();
          //  teacher.ChangeState(new ChillState(teacher));   


        }


    }

    private void SetNextTarget()
    {
        //null safegaurd
        if (teacher.petrolPoints == null && teacher.petrolPoints.Count == 0)
        {
            Debug.Log("No petrol points set");
            return;
        }
        currentTarget = teacher.petrolPoints[currentPetrolIndex];

        // null safeguard
        if (currentTarget == null)
        {
            Debug.Log("Current petrol point is null");
            return;
        }

        teacherAI.SetDestination(currentTarget.position);

    }

}

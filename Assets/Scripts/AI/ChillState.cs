using UnityEngine;

public class ChillState : ITeacherState
{
    private TeacherController teacher;
    private float chillDuration = 5.0f; 
    private float timer;
    private teacherAnimation teacherAnimation;
    public ChillState(TeacherController teacher)
    {
        this.teacher = teacher;
    }

    public void Enter()
    {
        Debug.Log("Teacher is chilling!");
        timer = 0f;

      
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= chillDuration)
        {
            teacher.ChangeState(new PetrolState(teacher));
        }

        
    }

    public void Exit()
    {
        Debug.Log("Teacher is done chilling!");
    }


}

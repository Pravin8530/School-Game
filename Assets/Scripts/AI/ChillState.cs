using UnityEngine;

public class ChillState : ITeacherState
{
    private TeacherController teacher;
    private float chillDuration = 5.0f; // Duration of the chill state in seconds
    private float timer;

    public ChillState(TeacherController teacher)
    {
        this.teacher = teacher;
    }

    public void Enter()
    {
        Debug.Log("Teacher is chilling!");
        timer = 0f;

        // Optionally, you can play a chill animation here
        // teacher.GetComponent<Animator>()?.SetTrigger("Chill");
    }

    public void Update()
    {
        timer += Time.deltaTime;

        // After the chill duration, transition to another state (e.g., PatrolState)
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

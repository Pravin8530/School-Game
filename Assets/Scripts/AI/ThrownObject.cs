using UnityEngine;

public class ThrownObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object hit the Teacher
        TeacherController teacher = collision.gameObject.GetComponent<TeacherController>();

        if (teacher != null)
        {
            // Find the player in the scene to pass their position
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 playerPos = player != null ? player.transform.position : transform.position;

            // Trigger stun on the teacher
            teacher.GetStunned(playerPos);

            //Destroy(gameObject);
        }
    }
}
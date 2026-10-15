using UnityEngine;

public class ThrownObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        TeacherController teacher = collision.gameObject.GetComponent<TeacherController>();

        if (teacher != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 playerPos = player != null ? player.transform.position : transform.position;

            teacher.GetStunned(playerPos);

        }
    }
}
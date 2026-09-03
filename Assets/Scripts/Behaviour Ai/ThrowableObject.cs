using UnityEngine;
using Unity.Behavior; // Required for Unity Behavior Graphs

public class ThrowableObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 1. Check if the object hit the Enemy AI
        if (collision.gameObject.CompareTag("Enemy"))
        { 
            Debug.Log("enemy get collided");
            // 2. Get the Behavior Graph Agent from the Enemy
            if (collision.gameObject.TryGetComponent<BehaviorGraphAgent>(out var agent))
            {
                // 3. Set IsStunned to true on the blackboard
                agent.SetVariableValue("IsStunned", true);

                // 4. Optionally pass the impact position to StunTargetLocation
                agent.SetVariableValue("StunTargetLocation", transform.position);
            }

            // Optional: Destroy or deactivate the throwable object on impact
            //Destroy(gameObject);
        }
    }
}
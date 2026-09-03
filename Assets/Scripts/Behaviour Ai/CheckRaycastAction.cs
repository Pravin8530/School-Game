using System;
using UnityEngine;
using Unity.Behavior;
using Action = Unity.Behavior.Action;
//using UnityEngine.Rendering;

public class CheckRaycastAction : MonoBehaviour
{
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 60f;
  
    public LayerMask playerLayer;
    public LayerMask obstacleMask;
    public Transform playerTransform;
     [SerializeField] private BehaviorGraphAgent behaviourAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
         
        if (CanSeePlayer())
        {
            behaviourAgent.SetVariableValue("CanSeePlayer", true);
            behaviourAgent.SetVariableValue("LastKnownPosition", playerTransform.transform.position);

        }
        else
        { 
            if(behaviourAgent!=null){
            behaviourAgent.SetVariableValue("CanSeePlayer", false);
            }
        }



    }

  public bool CanSeePlayer()
    {
        if (playerTransform == null) return false;

        // 1. DISTANCE CHECK
        Vector3 dirToPlayer = (playerTransform.position - transform.position);
        float distanceToPlayer = dirToPlayer.magnitude;

        if (distanceToPlayer <= viewDistance)
        {
            // 2. CONE ANGLE CHECK
            // Vector3.Angle checks the angle between AI forward vector and direction to player
            if (Vector3.Angle(transform.forward, dirToPlayer.normalized) < viewAngle / 2f)
            {
                // 3. RAYCAST (LINE OF SIGHT CHECK)
                // Raycast from AI to player to check for obstacle blocking
                if (!Physics.Raycast(transform.position + Vector3.up, dirToPlayer.normalized, distanceToPlayer, obstacleMask))
                {
                    // Player is in distance, inside cone, and NOT behind a wall
                    return true; 
                }
            }
        }

        return false;
    }

    // Draw FOV cone in Unity Editor Scene View for easy debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftBound = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightBound = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftBound * viewDistance);
        Gizmos.DrawRay(transform.position, rightBound * viewDistance);
    }

}

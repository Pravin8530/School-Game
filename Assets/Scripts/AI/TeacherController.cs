using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.AppUI.UI;
using Unity.Mathematics;

public class TeacherController : MonoBehaviour
{
    public GameObject player;
    public teacherAnimation teacherAnimation;
    public TeacherDebugUi teacherDebug;

    public TeacherAudio teacherAudio;
    public List<Transform> petrolPoints = new List<Transform>();

    private ITeacherState currentState;

    public ITeacherState CurrentState => currentState;

    [Header("Vision")]
    public float viewDistance = 50f;
    public float viewAngle = 90f;
    public LayerMask obstacleMask;

    [Header("Notice Delays")]
    public float firstNoticeTime = 2.0f; 
    public float quickNoticeTime = 0.2f; 
    public float memoryTime = 5.0f;      
    public Vector3 lastKnownPosition;
    public float lastSeenTimer = -999f;

    [Header("Hearing Settings")]
    public float hearingRadius; 
    [HideInInspector] public Vector3 soundLocation;
    [HideInInspector] public bool heardSoundThisFrame = false;

    public Vector3 investigationPoint; 


    [Header("Stun Settings")]
    public float stunDuration = 3f;

   

    public bool hasHeardSound = false;
   public bool isStunned = false;

    [Header("national antham")]
   public float nationalAnthamDuriation = 30f;
    private bool isLooking;
    Quaternion currentRotation;
    void Start()
    {
        ChangeState(new PetrolState(this));
    }


    public void ChangeState(ITeacherState userState)
    {
        currentState?.Exit();
        currentState = userState;
        currentState?.Enter();

    }

    void Update()
    {
        currentState?.Update();

        if (isLooking)
        {

            Vector3 directionVector3 = player.transform.position - transform.position;
            directionVector3.y = 0;


            currentRotation = transform.rotation;

            Quaternion direction = Quaternion.LookRotation(directionVector3);

            transform.rotation = Quaternion.Slerp(currentRotation, direction, 10f * Time.deltaTime);
             
            
        }
    }



    public bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 dirToPlayer = (player.transform.position - transform.position);
        float distanceToPlayer = dirToPlayer.magnitude;

        if (distanceToPlayer <= viewDistance)
        {
            if (Vector3.Angle(transform.forward, dirToPlayer.normalized) < viewAngle / 2f)
            {
                if (!Physics.Raycast(transform.position + Vector3.up, dirToPlayer.normalized, distanceToPlayer, obstacleMask))
                {
                    lastKnownPosition = player.transform.position; 
                    return true;
                }
            }
        }

        return false;
    }


    public void LookAtPlayer()
    {
        isLooking = true;
        Debug.Log("isLooking at the player");

    }





    public void OnHearSound(Vector3 noisePosition)
    {
        if (currentState is ChaseState || currentState is CatchState ) return;
        soundLocation = noisePosition;
        investigationPoint = soundLocation; 

       

        hasHeardSound = true;
        ChangeState(new SearchState(this));
    }


    public void GetStunned(Vector3 attackerPosition)
    {
        lastKnownPosition = attackerPosition;
        ChangeState(new StunState(this, stunDuration));

    }

     


    // gizmos and mesh 

    private Mesh CreateVisionConeMesh()
    {
        Mesh mesh = new Mesh();
        int segments = 20;

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        float currentAngle = -viewAngle / 2f;
        float deltaAngle = viewAngle / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angleRad = (currentAngle + transform.eulerAngles.y) * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(angleRad), 0, Mathf.Cos(angleRad)) * viewDistance;
            vertices[i + 1] = dir;
            currentAngle += deltaAngle;
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
    private void OnDrawGizmosSelected()
    {
        // Draw the yellow distance sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // Draw the solid red vision cone mesh
        Gizmos.color = new Color(1f, 0f, 0f, 0.35f); // Red with 35% transparency
        Mesh coneMesh = CreateVisionConeMesh();
        Gizmos.DrawMesh(coneMesh, transform.position, Quaternion.identity);

        // Add hearing circle visualization to OnDrawGizmosSelected

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // Blue sphere for Hearing Radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);

    }



}





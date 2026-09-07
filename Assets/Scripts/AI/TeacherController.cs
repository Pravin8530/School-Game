using UnityEngine;
//using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
//using Unity.AppUI.UI;

public class TeacherController : MonoBehaviour
{
    public GameObject player;

    public List<Transform> petrolPoints = new List<Transform>();

    private ITeacherState currentState;

    [Header("Vision")]
    public float viewDistance = 50f;
    public float viewAngle = 90f;
    public LayerMask obstacleMask;
    [Header("Notice Delays")]
    public float firstNoticeTime = 2.0f; // Long wait when first spotted
    public float quickNoticeTime = 0.2f; // Short wait if spotted again fast
    public float memoryTime = 5.0f;      // How long before teacher "forgets"
    public Vector3 lastKnownPosition;
    public float lastSeenTimer = -999f;

    [Header("Hearing Settings")]
    public float hearingRadius; // Max distance teacher can hear noises
    [HideInInspector] public Vector3 soundLocation;
    [HideInInspector] public bool heardSoundThisFrame = false;

    public Vector3 investigationPoint; // Point to investigate when a sound is heard;


    [Header("Stun Settings")]
    public float stunDuration = 3f; // Stun duration in seconds

    [Header("Debugging_Ui")]
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI canSeeText;
    public TextMeshProUGUI HeardSoundText;
    public TextMeshProUGUI stunedText;

   [SerializeField] public  bool hasHeardSound = false;
    [SerializeField] public bool isStunned = false;

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
        // Debug.Log(currentState);
        if (debugText != null)
        {
            debugText.text = "Current State: " + (currentState?.GetType().Name ?? "None");
        }
        if (canSeeText != null)
        {
            bool canSee = CanSeePlayer();
            canSeeText.text = "Can See Player: " + canSee;
            canSeeText.color = canSee ? Color.green : Color.red; // Optional visual color change
        }
         if (HeardSoundText != null)
        {
            HeardSoundText.text = "Heard Sound: " + hasHeardSound;
            HeardSoundText.color = hasHeardSound ? Color.green : Color.red; // Optional visual color change
        }
        if (stunedText != null)
        {
            stunedText.text = "Is Stunned: " + isStunned;
            stunedText.color = isStunned ? Color.green : Color.red; // Optional visual color change
        }
        
    }



    //  Vision Detection system..
    public bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 dirToPlayer = (player.transform.position - transform.position);
        float distanceToPlayer = dirToPlayer.magnitude;

        // 1. Distance Check
        if (distanceToPlayer <= viewDistance)
        {
            // 2. Angle Check
            if (Vector3.Angle(transform.forward, dirToPlayer.normalized) < viewAngle / 2f)
            {
                // 3. Line of Sight Raycast Check (Blocks through walls)
                if (!Physics.Raycast(transform.position + Vector3.up, dirToPlayer.normalized, distanceToPlayer, obstacleMask))
                {
                    lastKnownPosition = player.transform.position; // Save last known position
                    return true;
                }
            }
        }

        return false;
    }



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

    /// hearing sounds


    // when u hear sound if its in hearing radius
    public void OnHearSound(Vector3 noisePosition)
    {
        // dont fk up chase or catch 
        if (currentState is ChaseState || currentState is CatchState) return;

        soundLocation = noisePosition;
        
        investigationPoint = soundLocation; // Update investigation point to the noise location

        Debug.Log("Teacher heard a sound at: " + noisePosition);
 
       hasHeardSound = true;
        // Switch to search/investigate state instantly
        ChangeState(new SearchState(this));
    }


    // Call this method when a thrown object hits the teacher
    public void GetStunned(Vector3 attackerPosition)
    {
        // Record where the player was when they threw the item
        lastKnownPosition = attackerPosition;
    
        isStunned = true;
        // Switch to StunState with the set duration
        ChangeState(new StunState(this, stunDuration));
        
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





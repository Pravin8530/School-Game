using UnityEngine;
using TMPro;
using System.Collections.Generic;

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

    [Header("Debugging_Ui")]
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI canSeeText;
    public TextMeshProUGUI HeardSoundText;
    public TextMeshProUGUI stunedText;

    [SerializeField] public bool hasHeardSound = false;
    [SerializeField] public bool isStunned = false;

//-----------------------------Animation Settings--------------------------------

    [Header("Animation Settings")]
    public Animator animator;

    [Header("Animation Clips")]
    public AnimationClip patrolClip;
    public AnimationClip supriseClip;
    public AnimationClip searchClip;
    public AnimationClip chaseClip;
    public AnimationClip stunClip;
    public AnimationClip catchClip;



    public void PlayAnimation(AnimationClip clip, float crossfadeTime = 0.15f)
    {
        if (clip == null)
        {
            Debug.LogWarning("Animation clip is null. Cannot play animation.");
            return;
        }

        if (animator != null)
        {
            if (!animator.gameObject.activeInHierarchy)
            {
                animator.gameObject.SetActive(true);
            }

            if (!animator.enabled)
            {
                animator.enabled = true;
            }

            animator.CrossFade(clip.name, crossfadeTime);
            Debug.Log("Playing animation: " + clip.name);
        }
    }

    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found in children.");
            }
        }
    }
//--------------------------

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
        
        //debug txt thingy
        if (debugText != null)
        {
            debugText.text = "Current State: " + (currentState?.GetType().Name ?? "None");
        }
        if (canSeeText != null)
        {
            bool canSee = CanSeePlayer();
            canSeeText.text = "Can See Player: " + canSee;
            canSeeText.color = canSee ? Color.green : Color.red; 
        }
        if (HeardSoundText != null)
        {
            HeardSoundText.text = "Heard Sound: " + hasHeardSound;
            HeardSoundText.color = hasHeardSound ? Color.green : Color.red; 
        }
        if (stunedText != null)
        {
            stunedText.text = "Is Stunned: " + isStunned;
            stunedText.color = isStunned ? Color.green : Color.red; 
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



    public void OnHearSound(Vector3 noisePosition)
    {
        if (currentState is ChaseState || currentState is CatchState  /* || currentState is SearchState */) return;
        soundLocation = noisePosition;
        investigationPoint = soundLocation; 

        Debug.Log("Teacher heard a sound at: " + noisePosition);

        hasHeardSound = true;
        ChangeState(new SearchState(this));
    }


    public void GetStunned(Vector3 attackerPosition)
    {
        lastKnownPosition = attackerPosition;
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





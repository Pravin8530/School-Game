using UnityEngine;

namespace StarterAssets
{
    [RequireComponent(typeof(Pickable))]
    public class BananaPeelProximity : MonoBehaviour
    {
        [Header("Ground Check")]
        public LayerMask groundLayer;
        public float groundCheckDistance = 0.25f;

        [Header("Slip Detection")]
        public LayerMask playerLayer;
        public float slipRadius = 0.75f;
        public float armDelayAfterThrow = 0.35f;
        [Tooltip("Cooldown in seconds before this banana can slip someone again.")]
        public float rearmCooldown = 1.5f;
        public bool disableOnSlip = false;

        public bool onGround { get; private set; }

        private Pickable pickable;
        private float armedAtTime = Mathf.NegativeInfinity;
        private bool hasTriggeredSlip;
        private readonly Collider[] hitBuffer = new Collider[8];

        private void Awake()
        {
            pickable = GetComponent<Pickable>();
        }

        public void ArmAfterThrowOrDrop()
        {
            hasTriggeredSlip = false;
            armedAtTime = Time.time + armDelayAfterThrow;
        }

        public void ResetSlipState()
        {
            hasTriggeredSlip = false;
            armedAtTime = Time.time + rearmCooldown;
        }

        private void Update()
        {
            onGround = CheckOnGround();

            if (pickable != null && pickable.isPickedup)
            {
                hasTriggeredSlip = false;
                return;
            }

            if (hasTriggeredSlip) return;
            if (!onGround || Time.time < armedAtTime) return;

            int count = Physics.OverlapSphereNonAlloc(
                transform.position,
                slipRadius,
                hitBuffer,
                playerLayer,
                QueryTriggerInteraction.Ignore
            );

            for (int i = 0; i < count; i++)
            {
                FirstPersonController player = hitBuffer[i].GetComponentInParent<FirstPersonController>();
                if (player != null)
                {
                    hasTriggeredSlip = true;
                    player.TriggerFall();

                    if (disableOnSlip)
                    {
                        gameObject.SetActive(false);
                    }
                    else if (rearmCooldown > 0f)
                    {
                        Invoke(nameof(ResetSlipState), rearmCooldown);
                    }
                    break;
                }
            }
        }

        public bool CheckOnGround()
        {
            Vector3 origin = transform.position + Vector3.up * 0.08f;
            Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.green);
            return Physics.Raycast(origin, Vector3.down, out _, groundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, slipRadius);
        }
    }
}
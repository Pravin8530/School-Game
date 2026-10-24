using UnityEngine;

namespace StarterAssets
{
    public class BananaPeel : MonoBehaviour
    {
        public LayerMask groundLayer;
        public bool onGround;
        public float groundCheckDistance = 1f;

        private void OnTriggerEnter(Collider other)
        {
            FirstPersonController player = other.GetComponent<FirstPersonController>();


                if (IsOnGround() && player != null)
                {
                    player.TriggerFall();

                }
           
        }

        public bool IsOnGround()
        {

            RaycastHit hit;
            Vector3 origin = transform.position + Vector3.up * 0.1f;

            if (Physics.Raycast(origin, Vector3.down, out hit, groundCheckDistance, groundLayer))
            {
                onGround = true;
                return true;
            }
            onGround = false;
            return false;
        }
    }


}

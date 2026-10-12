using UnityEngine;

namespace StarterAssets
{
    public class BananaPeel : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("banana trigger");
            // Check if the object colliding with the banana is the player
            FirstPersonController player = other.GetComponent<FirstPersonController>();

            if (player != null)
            {
                player.TriggerFall();

            }
        }
    }
}

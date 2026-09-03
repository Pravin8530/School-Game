using UnityEngine;

public class NoiseOnImpact : MonoBehaviour
{
    [Header("Noise Settings")]
    public float noiseRadius = 15f;
    public float minVelocityThreshold = 2f; // Minimum force needed to trigger sound

    private Pickable pickable;

    private void Awake()
    {
        pickable = GetComponent<Pickable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Don't make noise if the item is currently held in the player's hand
        if (transform.parent != null) return;

        // Check impact force
        if (collision.relativeVelocity.magnitude >= minVelocityThreshold)
        {
            if (SoundManager.Instance != null && collision.contactCount > 0)
            {
                // Safely fetch contact point without array bugs
                Vector3 contactPoint = collision.GetContact(0).point;
                SoundManager.Instance.MakeNoise(contactPoint, noiseRadius);
                Debug.Log($"Impact Noise created at {contactPoint} with radius {noiseRadius}");
            }
        }
    }
}
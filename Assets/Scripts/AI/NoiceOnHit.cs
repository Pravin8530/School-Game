using UnityEngine;

public class NoiseOnHit : MonoBehaviour
{
    [Header("Noise Settings")]
    public float noiseRadius = 20f; // Loudness radius when struck
    public float minImpactForce = 1.5f;

    private void OnCollisionEnter(Collision collision)
    {
        // Calculate force of the object striking this item
        if (collision.relativeVelocity.magnitude >= minImpactForce)
        {
            if (SoundManager.Instance != null && collision.contactCount > 0)
            {
                // Safely fetch contact point
                Vector3 contactPoint = collision.GetContact(0).point;
                SoundManager.Instance.MakeNoise(contactPoint, noiseRadius);
                Debug.Log($"{gameObject.name} struck! Noise created at {contactPoint}");
            }
        }
    }
}
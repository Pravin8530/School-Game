using UnityEngine;

public class NoiseOnHit : MonoBehaviour
{
    [Header("Noise Settings")]
    public float noiseRadius = 20f; 
    public float minImpactForce = 1.5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= minImpactForce)
        {
            if (SoundManager.Instance != null && collision.contactCount > 0)
            {
                SoundManager.Instance.MakeNoise(transform.position, noiseRadius);
            }
        }
    }
}
using UnityEngine;

public class NoiseOnImpact : MonoBehaviour
{
    [Header("Noise Settings")]
    public float noiseRadius = 15f;
    public float minVelocityThreshold = 2f; 
    private Pickable pickable;

    private void Awake()
    {
        pickable = GetComponent<Pickable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transform.parent != null) return;

        if (collision.relativeVelocity.magnitude >= minVelocityThreshold)
        {
            if (SoundManager.Instance != null && collision.contactCount > 0)
            {
                SoundManager.Instance.MakeNoise(transform.position, noiseRadius);
            }
        }
    }
}
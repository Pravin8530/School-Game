using UnityEngine;

public class Window : MonoBehaviour
{
    public Rigidbody[] shards;

    float breakThreshold = 3f;

    public bool isWindowBroken = false;
    private SoundManager soundManager;
  
  private void Start()
    {
        if (soundManager == null)
        {
            soundManager = FindObjectOfType<SoundManager>();
        }
    }
  
    void OnCollisionEnter(Collision other)
    {
        float objectSpeed = other.relativeVelocity.magnitude;

        if (objectSpeed >= breakThreshold)
        {
            WindowBreak();
            
        }

    }

   
    public void WindowBreak()
    {  
        AddressablesAudioManager.Instance.PlaySfx("WindowBreaking");
        soundManager.MakeNoise(transform.position, 15f, true, Color.green);
        if(isWindowBroken)
             return;
        isWindowBroken = true;

        foreach (Rigidbody shard in shards)
        {
            shard.isKinematic = false;

            Vector3 randomDirection =
              - transform.right * Random.Range(1f, 2f) +
                Random.insideUnitSphere * 3f;

            shard.AddForce(randomDirection * 20f,ForceMode.Impulse);

            shard.AddTorque(Random.insideUnitSphere * 3f, ForceMode.Impulse);
            
        }
        
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }
    }
}
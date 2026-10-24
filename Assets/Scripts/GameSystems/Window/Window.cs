using UnityEngine;

public class Window : MonoBehaviour
{
    public Rigidbody[] shards;

    float breakThreshold = 3f;

    public bool isWindowBroken = false;

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
        if(isWindowBroken)
             return;
        isWindowBroken = true;

        foreach (Rigidbody shard in shards)
        {
            shard.isKinematic = false;

            Vector3 randomDirection =
              - transform.right * Random.Range(2f, 5f) +
                Random.insideUnitSphere * 3f;

            shard.AddForce(randomDirection * 20f,ForceMode.Impulse);

            shard.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
            
        }
        
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }
    }
}
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
        Debug.Log("broked those dumbass window glasses");
        foreach (Rigidbody shard in shards)
        {
            shard.isKinematic = false;

           //Debug.Log(shard.name);
            Vector3 randomDirection =
               -transform.forward * Random.Range(2f, 5f) +
                Random.insideUnitSphere * 2f;

            shard.AddForce(randomDirection * 20f,ForceMode.Impulse);

            shard.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);



        }
    }
}
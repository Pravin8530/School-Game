// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PeriodicTableCollapse : MonoBehaviour
// {
//     public static PeriodicTableCollapse instance;

//     [Header("Only this group will fall")]
//     public Transform tableGroup; // drag ONLY 'Periodic Table Ghost (1)' here

//     public float initialDelay = 0.4f;
//     public float perCubeDelay = 0.03f;
//     public bool randomizeOrder = true;
//     public float scatterForce = 1.5f;
//     public float torqueAmount = 3f;


//     public ElementSlot[] allSlots;

//     private void Awake()
//     {
//         instance = this;
//     }
//     public void TriggerCollapse()
//     {
//        // StartCoroutine(CollapseRoutine());
//     }


//     public void CheckCompletion()
//     {
//         foreach (var slot in allSlots)
//         {
//             if (!slot.isFilled) return; // not done yet
//         }

//         StartCoroutine(CollapseRoutine());
//     }

//     IEnumerator CollapseRoutine()
//     {
//         yield return new WaitForSeconds(initialDelay);

//         List<Rigidbody> allCubes = new List<Rigidbody>(tableGroup.GetComponentsInChildren<Rigidbody>());

//         foreach (var slot in allSlots)
//         {
//             Rigidbody[] slotCubes = slot.GetComponentsInChildren<Rigidbody>();
//             allCubes.AddRange(slotCubes);
//         }


//         if (randomizeOrder)
//         {
//             for (int i = allCubes.Count - 1; i > 0; i--)
//             {
//                 int j = Random.Range(0, i + 1);
//                 (allCubes[i], allCubes[j]) = (allCubes[j], allCubes[i]);
//             }
//         }

//         foreach (Rigidbody rb in allCubes)
//         {
//             DropCube(rb);
//             yield return new WaitForSeconds(perCubeDelay);
//         }



//         yield return new WaitForSeconds(3f);

//         // then clear them out of the player's way
//         foreach (Rigidbody rb in allCubes)
//         {
//             if (rb == null) continue; // safety in case any got destroyed already
//             Collider col = rb.GetComponent<Collider>();
//             if (col != null) col.enabled = false; // stop blocking movement
//             rb.isKinematic = true; // stop physics entirely, or Destroy(rb.gameObject) if you don't need them anymore
//         }
//     }





//     void DropCube(Rigidbody rb)
//     {
//         rb.isKinematic = false;
//         rb.useGravity = true;

//         Vector3 randomPush = new Vector3(
//             Random.Range(-scatterForce, scatterForce),
//             Random.Range(0.2f, scatterForce * 0.6f),
//             Random.Range(-scatterForce, scatterForce)
//         );
//         rb.AddForce(randomPush, ForceMode.Impulse);
//         rb.AddTorque(Random.insideUnitSphere * torqueAmount, ForceMode.Impulse);
//     }


//     private void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.F))
//         {
//             TriggerCollapse();
//         }
//     }
// }
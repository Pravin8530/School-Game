using System.Collections;
using JetBrains.Annotations;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

public class FlagEvent : MonoBehaviour, IInteractables
{
    public int Priority => 1;

    public TeacherController teacher;
    [SerializeField] bool isFlagAttached;

    public Vector3 startPosition;

    public Transform endPosition;

    public Transform salutePosition;
   [SerializeField] private float duriation = 3f;
    float time = 0f;
  
    public FlagSpawner flagSpawner;

    void Start()
    { 
       // flagSpawner = GetComponent<FlagSpawner>();
        startPosition = transform.position;
         
    }

    void Update()
    {
        if (isFlagAttached)
        {
           
            if (time < duriation)
            {
                time += Time.deltaTime;
               float  t =   time/duriation;
                transform.position = Vector3.Lerp(startPosition, endPosition.position, t);
               
                
                if(salutePosition == null)
                {
                    Debug.Log("salutePosition is null");

                }

                teacher.ChangeState(new PatrioticState(teacher, salutePosition));
               
            }
        }
    }

    public void Interact(InventoryNew inventory)
    {

        if (inventory == null)
            return;
        if (!isFlagAttached)
        {
            ItemData item = inventory.HeldItem;

            if (item != null && item.itemName != "Flag")
            {
                return;
            }


            GameObject flag = inventory.HeldObject;
            if (flag == null)
                return;
            inventory.RemoveHeldItem(flag);

            flag.transform.SetParent(this.transform);
            flag.transform.localPosition = new Vector3(0.5f, 0f, 0f);
            flag.transform.localRotation = Quaternion.identity;

            isFlagAttached = true;
            StartCoroutine(BackToOrigin(flag));
            flagSpawner.SpawnFlagsWithDelay();
          
        }
    }
      
    public IEnumerator BackToOrigin(GameObject flag)
    { 
         yield return new WaitUntil(()=> flag ==null); 

        yield  return new WaitForSeconds(duriation);
        Vector3 currentPos = transform.position;
        float returnTime= 0f;


         while(returnTime < duriation)
        {
            returnTime += Time.deltaTime;
            float t = returnTime/duriation;
            transform.position = Vector3.Lerp(currentPos,startPosition,t);
            yield return null;

        }
      time =0f;
      isFlagAttached = false;
      transform.position = startPosition;
  
    }

 }

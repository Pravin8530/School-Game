using System.Collections;
using System.Net;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;

public class SeeSaw : MonoBehaviour, IInteractables
{
    private int priority = 1;
    public int Priority => priority;
    public GameObject player;

    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject midPoint;

    [SerializeField] private GameObject endPoint;
    public float duriation = 3f;
    bool isJumping;
    void Start()
    {
        startPoint.transform.position = transform.position;


    }
    void Update()
    {

    }

    public void Interact(InventoryNew inventory)
    {
        StartCoroutine(JumpRoutine());


    }


    private IEnumerator JumpRoutine()
    {
        isJumping = true;
        float elaspedTime = 0;

        while (elaspedTime < duriation)
        {
            elaspedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elaspedTime / duriation);
            Vector3 p1 = Vector3.Lerp(startPoint.transform.position, midPoint.transform.position, t);
            Vector3 p2 = Vector3.Lerp(midPoint.transform.position, endPoint.transform.position, t);


            player.transform.position = Vector3.Lerp(p1, p2, t);

            yield return null;

        }

        player.transform.position = endPoint.transform.position;
        isJumping = false;
    }
}

using UnityEngine;
using UnityEngine.Scripting;

public class test : MonoBehaviour
{ 
    public GameObject target;
    public float radius = 5f;
    public Vector3 dir;
  private LineRenderer renderer;
  [SerializeField] private Transform[] points;

    void Awake()
    {
                 renderer =  GetComponent<LineRenderer>();

    }
    void Start()
    {
        
         renderer.startWidth =0.5f;
    }

    void Update()
    { 
         Vector3 heading = target.transform.position - transform.position;
         Vector3 dir = heading .normalized;
        float maxDistance = Mathf.Min(Vector3.Distance(transform.position, target.transform.position),radius); 
        renderer.positionCount =points.Length;
        for( int i =0; i< points.Length; i++)
        {
            renderer.SetPosition(i, points[i].position);

        }   
       
    }

    // void OnDrawGizmos()
    // {   Vector3 heading = target.transform.position - transform.position;
    //      Vector3 dir = heading .normalized;

    //     Gizmos.DrawLine(transform.position, target.transform.position);
    //     Gizmos.DrawSphere(transform.position,radius);
    // }
}

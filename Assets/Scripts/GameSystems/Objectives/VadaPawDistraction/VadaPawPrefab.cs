using UnityEngine;
using UnityEditor;
public class VadaPawPrefab : MonoBehaviour
{

    private TeacherController teacherController;
    private GameObject teacherTransform;
    private float detectionRadius = 8f;
    public LayerMask wallLayer;

    private LineRenderer line;

    void Start()
    {
        teacherTransform = GameObject.FindWithTag("Teacher");
        teacherController = FindObjectOfType<TeacherController>();
        line = GetComponent<LineRenderer>();
        line.positionCount = 20;
        line.startWidth = 0.05f;

    }


    public void Update()
    {
        if (teacherTransform != null)
        {
            Vector3 start = transform.position;
            Vector3 dir = (teacherTransform.transform.position - start).normalized;
            float maxLength = Mathf.Min(Vector3.Distance(start, teacherTransform.transform.position), detectionRadius);

            // Stop the smell line if a wall is in the way
            if (Physics.Raycast(start, dir, out RaycastHit hit, maxLength, wallLayer))
            {
                maxLength = hit.distance;
            }

            Vector3 end = start + dir * maxLength;
            Vector3 side = Vector3.Cross(dir, Vector3.up);

            for (int i = 0; i < 20; i++)
            {
                float t = i / 19f;
                float wave = Mathf.Sin((t * 15f) - (Time.time * 5f)) * 0.2f * Mathf.Sin(t * Mathf.PI);
                line.SetPosition(i, Vector3.Lerp(start, end, t) + side * wave);
            }
        }
        // notifyin treacher when hes under radius
        if (teacherController != null)
        {
            if (teacherTransform != null)
            {
                float distance = Vector3.Distance(transform.position, teacherTransform.transform.position);
                if (distance < detectionRadius)
                {
                    if (teacherController != null)
                    {
                        teacherController.ChangeState(new VadaPawState(teacherController, transform, gameObject));
                    }
                }
            }
        }
    }

    public void DestroyVadaPawPrefab()
    {
        Destroy(gameObject, 5f);
    }



    //range radius
    public void OnDrawGizmosSelected()
    {
        Color transparentYellow = Color.yellow;
        transparentYellow.a = 0.05f;
        Handles.color = transparentYellow;
        Handles.DrawSolidDisc(transform.position, Vector3.up, detectionRadius);
    }

}

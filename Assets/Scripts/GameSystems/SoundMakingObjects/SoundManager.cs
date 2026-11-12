using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public GameObject soundVisualizerPrefab; 
    public bool isSoundVisualizerEnabled = true; 
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void MakeNoise(Vector3 sourcePosition, float soundRadius, bool isSoundVisualizerEnabled)
    {
        if (soundVisualizerPrefab != null && isSoundVisualizerEnabled)
        { 
            
            GameObject ring = Instantiate(soundVisualizerPrefab, sourcePosition, Quaternion.identity);
            ring.GetComponent<SoundVisualizer>().Setup(soundRadius);
        }



        TeacherController teacher = FindFirstObjectByType<TeacherController>();

        if(teacher !=null)
        {
            float distanceToNoise = Vector3.Distance(teacher.transform.position, sourcePosition);

            if (distanceToNoise <= soundRadius)
            {
                teacher.OnHearSound(sourcePosition);
            }
        }
    }
}
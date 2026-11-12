using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public GameObject soundVisualizerPrefab; 
    public bool isSoundVisualizerEnabled = true; 
    public Color ringColor;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
       //soundVisualizerPrefab = getComponent<SoundManager>();
       // soundVisualizerPrefab.RingColor = 
    }
    public void MakeNoise(Vector3 sourcePosition, float soundRadius, bool isSoundVisualizerEnabled, Color? ringColor = null)
    {
        if (soundVisualizerPrefab != null && isSoundVisualizerEnabled)
        { 
            
            GameObject ring = Instantiate(soundVisualizerPrefab, sourcePosition, Quaternion.identity);
            ring.GetComponent<SoundVisualizer>().Setup(soundRadius);
            if (ringColor.HasValue)
            {
                ring.GetComponent<SoundVisualizer>().RingColor = ringColor.Value;
            }
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
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public GameObject soundVisualizerPrefab; // Assign a prefab with SoundVisualizer script in the Inspector
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void MakeNoise(Vector3 sourcePosition, float soundRadius)
    {
        if (soundVisualizerPrefab != null)
        {
            GameObject ring = Instantiate(soundVisualizerPrefab, sourcePosition, Quaternion.identity);
            ring.GetComponent<SoundVisualizer>().Setup(soundRadius);
        }
        TeacherController[] teachers = FindObjectsByType<TeacherController>(FindObjectsSortMode.None);

        foreach (TeacherController teacher in teachers)
        {
            float distanceToNoise = Vector3.Distance(teacher.transform.position, sourcePosition);

            if (distanceToNoise <= soundRadius)
            {
                teacher.OnHearSound(sourcePosition);
            }
        }
    }
}
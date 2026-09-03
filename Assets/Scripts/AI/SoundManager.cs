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

    // Call this anywhere to make a sound!
    public void MakeNoise(Vector3 sourcePosition, float soundRadius)
    {
        // Visualize the sound ring in the Editor
        if (soundVisualizerPrefab != null)
        {
            GameObject ring = Instantiate(soundVisualizerPrefab, sourcePosition, Quaternion.identity);
            ring.GetComponent<SoundVisualizer>().Setup(soundRadius);
        }
        // Find all Teachers in the scene
        TeacherController[] teachers = FindObjectsByType<TeacherController>(FindObjectsSortMode.None);

        foreach (TeacherController teacher in teachers)
        {
            float distanceToNoise = Vector3.Distance(teacher.transform.position, sourcePosition);

            // Check if teacher is close enough to hear it
            if (distanceToNoise <= soundRadius)
            {
                teacher.OnHearSound(sourcePosition);
            }
        }
    }
}
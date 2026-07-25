using UnityEngine;
using UnityEngine.Playables;

public class FPS : MonoBehaviour
{
    private float deltaTime;
    private GUIStyle style;


    private void Awake()
    {
        QualitySettings.vSyncCount = 0;    // Turn off VSync so the custom framerate can take effect

        Application.targetFrameRate = 60;       //Lock the target framerate to 60
    }

    void Start()
    {

        style = new GUIStyle();
        style.fontSize = 30; // Increase font size
        style.normal.textColor = Color.white;

    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int fps = Mathf.RoundToInt(1.0f / deltaTime);
        GUI.Label(new Rect(10, 10, 200, 50), "FPS: " + fps, style);
    }
}

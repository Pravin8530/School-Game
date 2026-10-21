using UnityEngine;

public class SoundVisualizer : MonoBehaviour
{
    private float maxRadius;
    private float currentRadius = 0f;
    private float expandSpeed = 15f;
    private float fadeSpeed = 2f;

    private LineRenderer lineRenderer;
    private Material lineMaterial;
    private Color ringColor = Color.cyan;

    public void Setup(float radius)
    {
        maxRadius = radius;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.015f;
        lineRenderer.endWidth = 0.015f;
        lineRenderer.positionCount = 40;

        lineMaterial = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material = lineMaterial;
    }

    void Update()
    {
        if (lineRenderer == null) return;

        if (currentRadius < maxRadius)
        {
            currentRadius += expandSpeed * Time.deltaTime;
        }

        DrawCircle(currentRadius);

        ringColor.a -= fadeSpeed * Time.deltaTime;
        lineRenderer.startColor = ringColor;
        lineRenderer.endColor = ringColor;

        if (ringColor.a <= 0f)
        {
            Destroy(gameObject);
        }
    }



    private void DrawCircle(float radius)
    {
        int segments = 60;
        lineRenderer.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * (360f / segments);

            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            Vector3 point = transform.position + (direction * radius);
            point.y += 0.05f;

            lineRenderer.SetPosition(i, point);
        }
    }


}

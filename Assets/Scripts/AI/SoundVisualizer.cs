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
        float angleStep = 360f / 39;

        for (int i = 0; i < 40; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 point = new Vector3(Mathf.Cos(angle) * radius, 0.05f, Mathf.Sin(angle) * radius);
            lineRenderer.SetPosition(i, transform.position + point);
        }
    }
}

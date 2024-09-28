using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LineRenderer), typeof(Collider2D))]
public class Shaper : MonoBehaviour
{
    private PolygonCollider2D _polygonCollider;
    private LineRenderer _lineRenderer;
    public Color color = Color.white;

    [Range(0.01f, 3f)] public float radius = 2f;
    [Range(0, 1.5f)] public float jaggedLow = 1.5f;
    [Range(0, 1.5f)] public float jaggedHigh = 1.5f;
    [Range(3, 150)] public int pointCountLow = 8;
    [Range(3, 150)] public int pointCountHigh = 12;
    [Range(0f, 1f)] public float maxChange = 0.5f;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _polygonCollider = GetComponent<PolygonCollider2D>();

        // Set white color
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    void Start()
    {
        GenerateLine();
    }

    private void OnValidate()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _polygonCollider = GetComponent<PolygonCollider2D>();

        GenerateLine();
    }

    void GenerateLine()
    {
        _lineRenderer.positionCount = 0;
        _lineRenderer.loop = true;

        // Calculate points for circle
        var pointCount = Random.Range(pointCountLow, pointCountHigh);
        _lineRenderer.positionCount = pointCount;
        var points = new Vector3[pointCount];
        var previousRadius = radius;

        // Generate points
        for (var i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / pointCount);
            var pointRadius = previousRadius * maxChange +
                              (radius + Random.Range(jaggedLow, jaggedHigh)) * (1f - maxChange);
            points[i] = new Vector3(Mathf.Sin(rad) * pointRadius, Mathf.Cos(rad) * pointRadius, 0);
            previousRadius = pointRadius;
        }

        // Add points to LineRenderer
        _lineRenderer.SetPositions(points);

        // Use the line-renderer's Vector3's to create vector2 collider path
        var path = new Vector2[_lineRenderer.positionCount + 1];
        for (var i = 0; i < pointCount; i++)
        {
            // Convert Vector3 to Vector2
            path[i] = points[i];
        }

        path[_lineRenderer.positionCount] = path[0];
        _polygonCollider.SetPath(0, path);
    }
}
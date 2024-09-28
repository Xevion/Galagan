using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    private PolygonCollider2D _polygonCollider;
    private LineRenderer _lineRenderer;
    public new Rigidbody2D rigidbody;
    
    [Range(0.1f, 10f)]
    public float radius = 2f;
    [Range(0.1f, 10f)]
    public float jaggedness = 1.5f;
    [SerializeField, Range(3, 150)]
    public int pointCountLow = 8;
    [SerializeField, Range(3, 150)]
    public int pointCountHigh = 12;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        
        // Set white color
        _lineRenderer.startColor = Color.white;
        _lineRenderer.endColor = Color.white;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    void Start()
    {
        GenerateLine();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateLine();
        }
    }   

    void GenerateLine()
    {
        _lineRenderer.positionCount = Random.Range(pointCountLow, pointCountHigh);
        _lineRenderer.loop = true;

        var previousRadius = radius;
        for (var i = 0; i < _lineRenderer.positionCount; i++)
        {
            var angle = i * Mathf.PI * 2 / _lineRenderer.positionCount;
            var pointRadius = previousRadius * 0.2f + (radius + Random.Range(0, jaggedness)) * 0.8f;
            _lineRenderer.SetPosition(i, new Vector3(pointRadius * Mathf.Cos(angle), pointRadius * Mathf.Sin(angle), 0));
            previousRadius = pointRadius;
        }
        
        // Use the line render to create collider path
        var path = new Vector2[_lineRenderer.positionCount + 1];
        for (var i = 0; i < _lineRenderer.positionCount; i++)
        {
            var p = _lineRenderer.GetPosition(i);
            path[i] = new Vector2(p.x, p.y);
        }
        path[_lineRenderer.positionCount] = path[0];
        
        _polygonCollider.SetPath(0, path);
    }
}

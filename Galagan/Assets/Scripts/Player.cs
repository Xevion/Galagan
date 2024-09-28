using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private PolygonCollider2D _polygonCollider;
    private Rigidbody2D _rigidbody;

    [Range(0.01f, 1.2f)] public float shipWidth = 1.3f;
    [Range(0.01f, 1.2f)] public float shipHeight = 0.8f;
    [Range(0.01f, 1.2f)] public float shipHeightRatio = 0.5f;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = Color.white;
        _lineRenderer.endColor = Color.white;
        _lineRenderer.useWorldSpace = false;
    }

    void Start()
    {
        GenerateShape();
    }

    void GenerateShape()
    {

        var points = new[]
        {
            new Vector2(0, shipHeight),
            new Vector2(-shipWidth / 2, -shipHeight),
            new Vector2(0, shipHeight * -shipHeightRatio),
            new Vector2(shipWidth / 2, -shipHeight),
            new Vector2(0, 0)
        };
        points[points.Length -1] = points[0];
        _polygonCollider.SetPath(0, points);

        _lineRenderer.loop = false;
        _lineRenderer.positionCount = _polygonCollider.points.Length;
        _lineRenderer.SetPositions(Array.ConvertAll(_polygonCollider.points,
            point => new Vector3(point.x, point.y, 0)));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player collided with " + collision.gameObject.name);
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal") * 0.4f, 0, 0);
    }
}
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private PolygonCollider2D _polygonCollider;
    private Rigidbody2D _rigidbody;

    private float _minX;
    private float _maxX;

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

    private void Start()
    {
        GenerateShape();

        var cam = Camera.main!;
        const float inset = 0.05f;
        _minX = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth * inset, 0, cam.nearClipPlane)).x;
        _maxX = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth * (1f - inset), 0, cam.nearClipPlane)).x;
    }

    private void GenerateShape()
    {
        var points = new[]
        {
            new Vector2(0, shipHeight),
            new Vector2(-shipWidth / 2, -shipHeight),
            new Vector2(0, shipHeight * -shipHeightRatio),
            new Vector2(shipWidth / 2, -shipHeight),
            new Vector2(0, 0)
        };
        points[^1] = points[0];
        _polygonCollider.SetPath(0, points);

        _lineRenderer.loop = false;
        _lineRenderer.positionCount = _polygonCollider.points.Length;
        _lineRenderer.SetPositions(Array.ConvertAll(_polygonCollider.points,
            point => new Vector3(point.x, point.y, 0)));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            SceneManager.LoadScene("Scenes/MenuScene", LoadSceneMode.Single);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = Vector2.zero;
        transform.position += new Vector3(Input.GetAxis("Horizontal") * 0.4f, 0, 0);

        // Prevent moving off-screen
        if (transform.position.x > _maxX)
            transform.position = new Vector3(_maxX, transform.position.y, transform.position.z);
        else if (transform.position.x < _minX)
            transform.position = new Vector3(_minX, transform.position.y, transform.position.z);
    }
}
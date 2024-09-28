using TMPro;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public Point pointPrefab;
    public float inset = 0.8f;
    private float _spawningLeft;
    private float _spawningRight;
    private float _spawningHeight;
    private int _asteroidCount;
    private int _pointCount;
    private int _layerMask;

    private int _score;
    public TextMeshProUGUI scoreText;

    public int Score
    {
        get => _score;
        set
        {
            if (scoreText == null)
                scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            
            _score = value;
            scoreText.text = $"{_score}";
        }
    }

    private void Start()
    {
        _layerMask = LayerMask.GetMask("Asteroid", "Points");
        
        InvokeRepeating(nameof(SpawnAsteroids), 0, 1.4f);
        InvokeRepeating(nameof(SpawnPoints), 0, 1f);
            
        // Get screen corners
        var cam = Camera.main;
        var topLeft = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        var topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight * inset, cam.nearClipPlane));
        _spawningRight = topRight.x * 0.8f;
        _spawningLeft = topLeft.x + (1f - inset) * _spawningRight;

        _spawningHeight = topLeft.y * 1.4f;
    }

    private void SpawnAsteroids()
    {
        Vector2 point = default;
        var attemptsLeft = 5;
        while (attemptsLeft > 0)
        {
            point = new Vector2(Random.Range(_spawningLeft, _spawningRight), _spawningHeight);
            if (Physics2D.OverlapPoint(point, _layerMask) == null)
                break;
            attemptsLeft--;
        }
        
        if (attemptsLeft < 5)
        {
            Debug.LogWarning("Multiple attempts to find a spawn point");
            if (attemptsLeft == 0)
            {
                Debug.LogError("Failed to find a spawn point");
                return;
            }
        }
        
        var asteroid = Instantiate(asteroidPrefab, point,  new Quaternion());
        asteroid.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1, 1f) * 20, Random.Range(-90, -110)));
        asteroid.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * 10);
        asteroid.name = $"Asteroid {_asteroidCount}";
        _asteroidCount++;
    }

    private void SpawnPoints()
    {
        var x = Random.Range(_spawningLeft, _spawningRight);
        var point = Instantiate(pointPrefab, new Vector3(x, _spawningHeight, 0),  new Quaternion());
        point.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1, 1f) * 20, Random.Range(-90, -110)));
        point.name = $"Point {_pointCount}";
        _pointCount++;
    }
}
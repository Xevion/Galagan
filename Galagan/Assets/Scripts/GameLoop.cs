using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

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
    
    private AudioClip[] _audioClips;

    private AudioSource _audioSource;

    public int Score
    {
        get => _score;
        set
        {
            if (scoreText == null)
                scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            
            _score = value;
            scoreText.text = $"{Score}";

            if (value > 0)
            {
                var index = value <= 27 ? value : Random.Range(1, 28);
                _audioSource.PlayOneShot(_audioClips[index - 1]);
            }
        }
    }

    private IEnumerator LoadAudio()
    {
        var assetPath = $"file://{Application.streamingAssetsPath}/Audio/Sounds";
        for (var i = 1; i <= 27; i++)
        {
            var www = UnityWebRequestMultimedia.GetAudioClip($"{assetPath}/{i}.wav", AudioType.WAV);
            yield return www.SendWebRequest();
            
            if (www.error != null)
            {
                Debug.LogError($"Failed to load audio clip {i}: {www.error}");
                continue;
            }
            
            _audioClips[i - 1] = DownloadHandlerAudioClip.GetContent(www);
            _audioClips[i - 1].name = $"{i}.wav";
            
            if (i == 27)
            {
                Debug.Log("All audio clips loaded");
            }
        }
    }

    private void Start()
    {
        _layerMask = LayerMask.GetMask("Asteroid", "Points");
        _audioSource = GetComponent<AudioSource>();
        _audioClips = new AudioClip[27];
        
        StartCoroutine(LoadAudio());
        
        InvokeRepeating(nameof(SpawnAsteroids), 0, 1.4f);
        InvokeRepeating(nameof(SpawnPoints), 0, 1f);
            
        // Get screen corners
        var cam = Camera.main!;
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
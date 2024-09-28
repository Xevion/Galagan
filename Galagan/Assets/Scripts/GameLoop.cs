using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    private float spawningLeft;
    private float spawningRight;
    private float spawningHeight;
    public float inset = 0.8f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnAsteroids), 0, 1.4f);
            
        // Get screen corners
        var cam = Camera.main;
        var topLeft = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        var topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight * inset, cam.nearClipPlane));
        spawningRight = topRight.x * 0.8f;
        spawningLeft = topLeft.x + (1f - inset) * spawningRight;

        spawningHeight = topLeft.y * 1.4f;
    }

    private void SpawnAsteroids()
    {
        var x = Random.Range(spawningLeft, spawningRight);
        var asteroid = Instantiate(asteroidPrefab, new Vector3(x, spawningHeight, 0),  new Quaternion());
        asteroid.rigidbody.AddForce(new Vector2(Random.Range(-1, 1f) * 20, Random.Range(-90, -110)));
        asteroid.rigidbody.AddTorque(Random.Range(-1f, 1f) * 10);
    }
}
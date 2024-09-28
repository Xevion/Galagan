using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private bool _correcting;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // If asteroids get too slow or stop, begin adding force to correct
        if (_rigidbody.velocity.y > -0.1f)
        {
            _correcting = true;
        }

        if (_correcting)
        {
            if (_rigidbody.velocity.y > -2f)
            {
                _rigidbody.AddForce(new Vector2(0, -5f));
            }
            else
            {
                // If the asteroid is now moving fast enough, stop correcting
                _correcting = false;
            }
        }
    }
}
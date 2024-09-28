using UnityEngine;

public class Point : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            
            // Find GameLoop object
            var gameLoop = GameObject.Find("GameLoop").GetComponent<GameLoop>();
            gameLoop.Score++;
        }
    }
}
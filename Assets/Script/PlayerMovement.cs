using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f; // Pac-Man's speed
    private Vector2 direction = Vector2.zero; // Pac-Man's current direction

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        
        // Get input from arrow keys or WASD
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Set Pac-Man's direction based on input
        if (horizontalInput > 0)
        {
            direction = Vector2.right;
        }
        else if (horizontalInput < 0)
        {
            direction = Vector2.left;
        }
        else if (verticalInput > 0)
        {
            direction = Vector2.up;
        }
        else if (verticalInput < 0)
        {
            direction = Vector2.down;
        }

        // Move Pac-Man in the current direction
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
}

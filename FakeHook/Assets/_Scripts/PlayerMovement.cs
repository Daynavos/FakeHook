using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from Unity's input system (WASD / Arrow keys)
        movement.x = Input.GetAxisRaw("Horizontal"); // -1 to 1
        movement.y = Input.GetAxisRaw("Vertical");   // -1 to 1
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
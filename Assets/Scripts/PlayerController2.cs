using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float jumpForce = 10f;
    public float rotationSpeed = 100f;

    private Rigidbody2D rb;
    private bool isLocked;
    private Vector2 jumpDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Lock to surface and stop movement
            isLocked = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0; // Ensure the player stays locked
        }

        if (Input.GetMouseButton(0))
        {
            // Prepare to jump (load jump)
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            jumpDirection = (mousePosition - rb.position).normalized;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Jump
            isLocked = false;
            rb.gravityScale = 1; // Apply gravity when jumping
            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        }

        if (!isLocked)
        {
            // Rotate the player based on movement direction
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            if (rb.velocity.magnitude > 0.1f) // To prevent jittering when velocity is almost zero
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            // Lock only if the side of the square touches, not the corner
            Vector2 contactNormal = collision.contacts[0].normal;
            if (Mathf.Abs(contactNormal.x) < 0.1f || Mathf.Abs(contactNormal.y) < 0.1f)
            {
                isLocked = true;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0; // Ensure the player stays locked
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Unlock when leaving the surface
        isLocked = false;
        rb.gravityScale = 1; // Apply gravity when not locked
    }
}

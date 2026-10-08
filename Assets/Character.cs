using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Character : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float jumpForce = 14f;
    [SerializeField] protected LayerMask groundLayer;

    [Header("Vertical Wrap")]
    [SerializeField] float wrapBottomY = -6f;
    [SerializeField] float wrapTopY = 6f;

    protected Rigidbody2D rb;
    protected Collider2D col;
    protected int facing = 1;

    public bool IsGrounded { get; private set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        if (transform.position.y < wrapBottomY)
            transform.position = new Vector3(transform.position.x, wrapTopY, 0f);
    }

    protected virtual void FixedUpdate()
    {
        Bounds b = col.bounds;
        RaycastHit2D hit = Physics2D.BoxCast(
            new Vector2(b.center.x, b.min.y),
            new Vector2(b.size.x * 0.8f, 0.05f),
            0f, Vector2.down, 0.1f, groundLayer);
        IsGrounded = hit && rb.linearVelocity.y <= 0.1f;
    }

    protected void Move(float dir)
    {
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
        if (dir != 0f) facing = dir > 0f ? 1 : -1;
    }

    protected void Jump()
    {
        if (!IsGrounded) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}
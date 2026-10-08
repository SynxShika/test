using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bubble : MonoBehaviour
{
    [Header("Flight")]
    [SerializeField] float shootSpeed = 9f;
    [SerializeField] float shootTime = 0.35f;
    [SerializeField] float floatSpeed = 1.5f;
    [SerializeField] float ceilingY = 3.4f;
    [SerializeField] float maxX = 8.4f;

    [Header("Lifetime")]
    [SerializeField] float emptyLifetime = 6f;
    [SerializeField] float trappedLifetime = 7f;

    [SerializeField] Color trappedColor = new Color(1f, 0.6f, 0.6f, 0.6f);

    Rigidbody2D rb;
    SpriteRenderer sr;
    ITrappable trapped;
    int dir = 1;
    float spawnTime, expireTime, swayOffset;
    bool floating, popped;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Launch(int direction)
    {
        dir = direction;
        spawnTime = Time.time;
        expireTime = spawnTime + emptyLifetime;
        swayOffset = Random.value * 10f;
    }

    void FixedUpdate()
    {
        if (!floating && Time.time - spawnTime >= shootTime) floating = true;

        if (!floating)
        {
            rb.linearVelocity = new Vector2(dir * shootSpeed, 0f);
        }
        else
        {
            float vy = rb.position.y < ceilingY ? floatSpeed : 0f;
            float vx = Mathf.Sin((Time.time + swayOffset) * 2f) * 0.6f;
            rb.linearVelocity = new Vector2(vx, vy);
        }

        rb.position = new Vector2(Mathf.Clamp(rb.position.x, -maxX, maxX), rb.position.y);

        if (Time.time >= expireTime) Expire();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (trapped != null || popped) return;
        if (other.TryGetComponent(out ITrappable t) && !t.IsTrapped)
            Capture(t);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (popped || Time.time - spawnTime < 0.4f) return;
        if (other.TryGetComponent(out PlayerController _))
            Pop();
    }

    void Capture(ITrappable t)
    {
        trapped = t;
        t.Trap(this);
        floating = true;
        sr.color = trappedColor;
        expireTime = Time.time + trappedLifetime;
    }

    void Pop()
    {
        popped = true;
        if (trapped != null) trapped.Defeat();
        else GameSession.Instance.AddPoints(10);
        Destroy(gameObject);
    }

    void Expire()
    {
        popped = true;
        if (trapped != null) trapped.Release();
        Destroy(gameObject);
    }
}
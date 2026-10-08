using UnityEngine;

public abstract class Enemy : Character, ITrappable
{
    [Header("Enemy")]
    [SerializeField] int points = 100;

    protected int direction = 1;
    Vector3 originalScale;

    public bool IsTrapped { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        originalScale = transform.localScale;
    }

    void Start()
    {
        GameSession.Instance.RegisterEnemy();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (IsTrapped || !GameSession.Instance.IsPlaying) return;
        Think();
        Move(direction);
    }

    // Each enemy type decides how it behaves.
    protected abstract void Think();

    protected bool HitWall()
    {
        Vector2 origin = col.bounds.center;
        float dist = col.bounds.extents.x + 0.1f;
        return Physics2D.Raycast(origin, Vector2.right * direction, dist, groundLayer);
    }

    void OnCollisionStay2D(Collision2D c)
    {
        if (IsTrapped) return;
        if (c.collider.TryGetComponent(out PlayerController p)) p.Hit();
    }

    public void Trap(Bubble bubble)
    {
        IsTrapped = true;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        transform.SetParent(bubble.transform);
        transform.localPosition = Vector3.zero;
        transform.localScale = originalScale * 0.6f;
    }

    public void Release()
    {
        IsTrapped = false;
        transform.SetParent(null);
        transform.localScale = originalScale;
        rb.simulated = true;
    }

    public void Defeat()
    {
        GameSession.Instance.EnemyDefeated(points);
        FruitFactory.Create(FruitFactory.RandomType(), transform.position);
        Destroy(gameObject);
    }
}

using UnityEngine;

public class PlayerController : Character
{
    BubbleShooter shooter;
    Vector3 spawnPoint;
    float invulnerableUntil;

    protected override void Awake()
    {
        base.Awake();
        shooter = GetComponent<BubbleShooter>();
        spawnPoint = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        if (!GameSession.Instance.IsPlaying) return;

        if (Input.GetButtonDown("Jump")) Jump();
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.LeftControl))
            shooter.Shoot(facing);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        float h = GameSession.Instance.IsPlaying ? Input.GetAxisRaw("Horizontal") : 0f;
        Move(h);
    }

    public void Hit()
    {
        if (Time.time < invulnerableUntil || !GameSession.Instance.IsPlaying) return;
        GameSession.Instance.LoseLife();
        transform.position = spawnPoint;
        rb.linearVelocity = Vector2.zero;
        invulnerableUntil = Time.time + 2f;
    }
}

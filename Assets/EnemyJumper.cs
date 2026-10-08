using UnityEngine;

public class EnemyJumper : Enemy
{
    Transform target;
    float nextJump, nextRetarget;

    void OnEnable()
    {
        PlayerController p = FindFirstObjectByType<PlayerController>();
        if (p != null) target = p.transform;
    }

    protected override void Think()
    {
        if (target != null && Time.time >= nextRetarget)
        {
            direction = target.position.x > transform.position.x ? 1 : -1;
            nextRetarget = Time.time + 1f;
        }

        if (HitWall()) direction = -direction;

        if (IsGrounded && Time.time >= nextJump)
        {
            Jump();
            nextJump = Time.time + Random.Range(1.5f, 3.5f);
        }
    }
}

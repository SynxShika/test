using UnityEngine;

public class EnemyWalker : Enemy
{
    float nextTurn;

    protected override void Awake()
    {
        base.Awake();
        nextTurn = Time.time + Random.Range(2f, 5f);
    }

    protected override void Think()
    {
        if (HitWall() || Time.time >= nextTurn)
        {
            direction = -direction;
            nextTurn = Time.time + Random.Range(2f, 5f);
        }
    }
}
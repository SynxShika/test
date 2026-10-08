using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    [SerializeField] Bubble bubblePrefab;
    [SerializeField] float cooldown = 0.4f;
    float nextShot;

    public void Shoot(int dir)
    {
        if (Time.time < nextShot) return;
        nextShot = Time.time + cooldown;

        Vector3 pos = transform.position + new Vector3(dir * 0.8f, 0f, 0f);
        Bubble b = Instantiate(bubblePrefab, pos, Quaternion.identity);
        b.Launch(dir);
    }
}
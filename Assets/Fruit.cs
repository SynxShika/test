using UnityEngine;

public class Fruit : MonoBehaviour
{
    int points;

    public void Setup(int p, Color color, float size)
    {
        points = p;
        GetComponent<SpriteRenderer>().color = color;
        transform.localScale = Vector3.one * size;
    }

    void Start()
    {
        Destroy(gameObject, 10f);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.collider.TryGetComponent(out PlayerController _))
        {
            GameSession.Instance.AddPoints(points);
            Destroy(gameObject);
        }
    }
}
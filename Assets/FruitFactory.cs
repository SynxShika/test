using UnityEngine;

public enum FruitType { Orange, Banana, Grape }

public static class FruitFactory
{
    static Fruit prefab;

    public static FruitType RandomType()
    {
        return (FruitType)Random.Range(0, 3);
    }

    public static Fruit Create(FruitType type, Vector3 position)
    {
        if (prefab == null) prefab = Resources.Load<Fruit>("Fruit");

        Fruit fruit = Object.Instantiate(prefab, position, Quaternion.identity);
        switch (type)
        {
            case FruitType.Orange: fruit.Setup(200, new Color(1f, 0.55f, 0f), 0.4f); break;
            case FruitType.Banana: fruit.Setup(300, Color.yellow, 0.5f); break;
            case FruitType.Grape: fruit.Setup(500, new Color(0.6f, 0.2f, 0.9f), 0.45f); break;
        }
        return fruit;
    }
}
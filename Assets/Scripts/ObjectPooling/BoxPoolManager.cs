using System.Collections.Generic;
using UnityEngine;

public class BoxPoolManager : MonoBehaviour, IObjectPool
{
    public GameObject redBoxPrefab;
    public GameObject blueBoxPrefab;
    public int initialPoolSize = 10;

    private Queue<Box> redBoxPool = new Queue<Box>();
    private Queue<Box> blueBoxPool = new Queue<Box>();

    private void Awake()
    {
        Preload(redBoxPrefab, redBoxPool, initialPoolSize);
        Preload(blueBoxPrefab, blueBoxPool, initialPoolSize);
    }

    void Preload(GameObject prefab, Queue<Box> pool, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj.GetComponent<Box>());
        }
    }

    public Box Spawn(Box.BoxColor color, Vector2 position)
    {
        Queue<Box> pool = (color == Box.BoxColor.Red) ? redBoxPool : blueBoxPool;
        GameObject prefab = (color == Box.BoxColor.Red) ? redBoxPrefab : blueBoxPrefab;

        Box box;
        if (pool.Count > 0)
        {
            box = pool.Dequeue();
        }
        else
        {
            box = Instantiate(prefab).GetComponent<Box>();
        }

        box.transform.position = position;
        box.Initialize(color);
        box.gameObject.SetActive(true);

        return box;
    }

    public void Despawn(Box box)
    {
        box.Despawn();

        if (box.boxColor == Box.BoxColor.Red)
            redBoxPool.Enqueue(box);
        else
            blueBoxPool.Enqueue(box);
    }
}

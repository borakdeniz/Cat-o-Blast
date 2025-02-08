using UnityEngine;
using System.Collections.Generic;

public class GemPool : MonoBehaviour
{
    public GameObject gemPrefab;
    private Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject GetGem()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return Instantiate(gemPrefab);
        }
    }

    public void ReturnGem(GameObject gem)
    {
        gem.SetActive(false);
        pool.Enqueue(gem);
    }
}

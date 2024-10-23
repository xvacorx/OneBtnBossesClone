using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ObjectPool[] objectPools = GetComponentsInChildren<ObjectPool>();

        foreach (var pool in objectPools)
        {
            pools.Add(pool.poolName, pool);
        }
    }

    public GameObject GetObject(string poolName)
    {
        if (pools.TryGetValue(poolName, out var pool))
        {
            return pool.GetObject();
        }

        Debug.LogWarning($"Pool {poolName} no encontrada.");
        return null;
    }
}

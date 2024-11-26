using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectFactory
{
    public static GameObject CreateObject(string poolName, Vector2 position, Quaternion rotation = default)
    {
        GameObject obj = PoolManager.Instance.GetObject(poolName);

        if (obj == null)
        {
            Debug.LogError($"No se pudo encontrar un objeto en el pool {poolName}");
            return null;
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }
}
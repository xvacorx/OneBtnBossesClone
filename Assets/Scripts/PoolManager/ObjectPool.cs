using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public string poolName;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 5;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    private void Awake()
    {
        poolName = this.gameObject.name;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform); 
            obj.SetActive(false); 
            availableObjects.Enqueue(obj); 
        }
    }

    public GameObject GetObject()
    {
        if (availableObjects.Count > 0)
        {
            GameObject obj = availableObjects.Dequeue();
            //obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject newObj = Instantiate(prefab, transform);
            //newObj.SetActive(true);
            return newObj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        availableObjects.Enqueue(obj);
    }
}

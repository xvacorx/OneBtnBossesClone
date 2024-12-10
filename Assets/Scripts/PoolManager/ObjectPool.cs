using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ObjectPool : MonoBehaviour
{
    public string poolName;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 5;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    private void Awake()
    {
        poolName = gameObject.name;
        InitializePool();
        DeactivateAllChildren();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DeactivateAllChildren();
    }
    private void InitializePool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateAndDeactivateObject();
        }
    }

    private void CreateAndDeactivateObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        availableObjects.Enqueue(obj);
    }

    public GameObject GetObject()
    {
        if (availableObjects.Count > 0)
        {
            GameObject obj = availableObjects.Dequeue();

            if (obj.activeInHierarchy)
            {
                obj = Instantiate(prefab, transform);
            }

            return obj;
        }
        else
        {
            return Instantiate(prefab, transform);
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        availableObjects.Enqueue(obj);
    }
    public void DeactivateAllChildren()
    {
        foreach (Transform child in transform)
        {
            ReturnObject(child.gameObject);
        }
    }
}

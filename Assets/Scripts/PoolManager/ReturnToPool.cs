using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReturnToPool : MonoBehaviour
{
    private void OnDisable()
    {
        ObjectPool pool = GetComponentInParent<ObjectPool>();
        if (pool != null)
        {
            pool.ReturnObject(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerProjectile : MonoBehaviour
{
    public Transform target;
    public float speed;
    private void Start()
    {
        Vector3 direction = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
    private void Update()
    {
        float projectileSpeed = speed * Time.deltaTime;
        transform.Translate(transform.forward * projectileSpeed * 2, Space.World);
    }
}

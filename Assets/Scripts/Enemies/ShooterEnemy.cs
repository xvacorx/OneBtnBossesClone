using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    public float fireRate = 1f;
    private float nextFireTime = 0f;
    [SerializeField] private string projectilePoolName = "ProjectilePool";

    public Transform target;
    public GameObject bulletGameObject;
    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (target == null)
        {
            return;
        }
        
        GameObject projectile = PoolManager.Instance.GetObject(projectilePoolName);
        if (projectile != null)
        {
            BasicProjectile bullet = projectile.GetComponent<BasicProjectile>();
            if (bullet != null)
            {
                bullet.enabled = false;
                bullet.target = target;
                projectile.transform.position = transform.position;
                bullet.enabled = true;
                projectile.SetActive(true);
                nextFireTime = Time.time + fireRate;
            }
        }
    }
}
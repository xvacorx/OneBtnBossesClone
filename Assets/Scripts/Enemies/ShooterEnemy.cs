using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    public float fireRate = 1f;
    private float nextFireTime = 0f;
    [SerializeField] private string projectilePoolName = "ProjectilePool";

    public Transform target;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject projectile = PoolManager.Instance.GetObject(projectilePoolName);
        if (projectile != null)
        {
            PlayerProjectile bullet = projectile.GetComponent<PlayerProjectile>();
            if (bullet != null)
            {
                bullet.target = target;

                projectile.transform.position = transform.position;
                projectile.SetActive(true);

                nextFireTime = Time.time + fireRate;
            }
        }
    }
}

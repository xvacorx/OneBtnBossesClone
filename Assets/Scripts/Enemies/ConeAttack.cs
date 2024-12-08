using System.Collections;
using UnityEngine;

public class ConeAttack : Obstacles
{
    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(SpawnCone), obstacleSpawnInterval, obstacleSpawnInterval);
    }

    private void SpawnCone()
    {
        float spawnAngle = Random.Range(0, 360);
        Vector2 spawnPosition = GetRandomPosition();
        GameObject cone = PoolManager.Instance.GetObject(obstaclePoolName);

        if (cone != null)
        {
            cone.transform.position = spawnPosition;
            cone.transform.rotation = Quaternion.Euler(0, 0, spawnAngle);
            Collider2D obstacleCollider = cone.GetComponent<Collider2D>();
            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = false;
                cone.SetActive(true);
                StartCoroutine(BlinkBeforeActivation(cone, obstacleCollider, colliderActivationDelay));
            }

            StartCoroutine(ReturnToPool(cone, obstacleLifeTime + colliderActivationDelay));
        }
    }
}
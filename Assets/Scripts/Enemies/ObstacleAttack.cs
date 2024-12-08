using System.Collections;
using UnityEngine;

public class ObstacleAttack : Obstacles
{
    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(SpawnObstacle), obstacleSpawnInterval, obstacleSpawnInterval);
    }

    private void SpawnObstacle()
    {
        Vector2 spawnPosition = GetRandomPosition();
        GameObject obstacle = PoolManager.Instance.GetObject(obstaclePoolName);

        if (obstacle != null)
        {
            obstacle.transform.position = spawnPosition;
            Collider2D obstacleCollider = obstacle.GetComponent<Collider2D>();
            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = false;
                obstacle.SetActive(true);
                StartCoroutine(BlinkBeforeActivation(obstacle, obstacleCollider, colliderActivationDelay));
            }

            StartCoroutine(ReturnToPool(obstacle, obstacleLifeTime + colliderActivationDelay));
        }
    }


}

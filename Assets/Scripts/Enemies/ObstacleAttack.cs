using System.Collections;
using UnityEngine;

public class ObstacleAttack : Obstacles
{
    [Header("Obstacle Settings")]
    [SerializeField] private string obstaclePoolName = "ObstaclePool";
    [SerializeField] private float obstacleSpawnInterval = 3f;
    [SerializeField] private float colliderActivationDelay = 0.5f;
    [SerializeField] private float obstacleLifeTime = 1f;

    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(SpawnObstacle), obstacleSpawnInterval, obstacleSpawnInterval);
    }
    private void SpawnObstacle()
    {
        Vector2 spawnPosition = GetRandomPosition();
        GameObject obstacle = GameObjectFactory.CreateObject(obstaclePoolName, spawnPosition);

        if (obstacle != null)
        {
            Collider2D obstacleCollider = obstacle.GetComponent<Collider2D>();
            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = false;
                StartCoroutine(ActivateObstacleCollider(obstacleCollider, colliderActivationDelay));
            }

            StartCoroutine(ReturnToPool(obstacle, obstacleLifeTime + colliderActivationDelay));
        }
    }


}

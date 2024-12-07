using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAttack : Obstacles
{
    [Header("Cone Settings")]
    [SerializeField] private string conePoolName = "ConePool";
    [SerializeField] private float coneSpawnInterval = 3f;
    [SerializeField] private float spawnAngle;
    [SerializeField] private float coneLifeTime = 1f;
    [SerializeField] private float colliderActivationDelay = 0.5f;

    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(SpawnCone), coneSpawnInterval, coneSpawnInterval);
    }
    private void SpawnCone()
    {
        spawnAngle = Random.Range(0, 360);
        Vector2 spawnPosition = GetRandomPosition();
        Quaternion spawnRotation = Quaternion.Euler(0, 0, spawnAngle);

        GameObject cone = GameObjectFactory.CreateObject(conePoolName, spawnPosition, spawnRotation);

        if (cone != null)
        {
            StartCoroutine(ReturnToPool(cone, coneLifeTime));
        }
        if (cone != null)
        {
            Collider2D obstacleCollider = cone.GetComponent<Collider2D>();
            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = false;
                StartCoroutine(ActivateObstacleCollider(obstacleCollider, colliderActivationDelay));
            }

            StartCoroutine(ReturnToPool(cone, coneLifeTime + colliderActivationDelay));
        }
    }
}

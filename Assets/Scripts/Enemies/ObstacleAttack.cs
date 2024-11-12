using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAttack : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject obstaclePrefab;
    public float spawnRadius = 3f;
    public float spawnInterval = 3f;
    public float colliderActivationDelay = 0.5f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating(nameof(SpawnObstacle), spawnInterval, spawnInterval);
    }

    private void SpawnObstacle()
    {
        Vector2 spawnPosition = GetRandomPosition();
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        Collider2D obstacleCollider = obstacle.GetComponent<Collider2D>();
        if (obstacleCollider != null)
        {
            obstacleCollider.enabled = false;
            Invoke(nameof(ActivateObstacleCollider), colliderActivationDelay);
        }
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 randomPosition;
        float distance;

        do
        {
            float angle = Random.Range(0, 2 * Mathf.PI);
            float x = Mathf.Cos(angle) * spawnRadius;
            float y = Mathf.Sin(angle) * spawnRadius;
            randomPosition = new Vector2(x, y);

            distance = Vector2.Distance(randomPosition, player.position);

        } while (distance < 1f);

        return randomPosition;
    }

    private void ActivateObstacleCollider()
    {
        Collider2D[] colliders = FindObjectsOfType<Collider2D>();
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Obstacle"))
            {
                collider.enabled = true;
            }
        }
    }
}
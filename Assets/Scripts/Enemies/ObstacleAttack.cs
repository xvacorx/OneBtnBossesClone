using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAttack : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float colliderActivationDelay = 0.5f;
    [SerializeField] private float lifeTime = 1f;
    private float spawnRadius;

    [Header("Cone Settings")]
    [SerializeField] private bool isCone;
    [SerializeField] private float spawnAngle;

    private GameObject player;
    private PlayerMovement movement;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = player.GetComponent<PlayerMovement>();
        spawnRadius = movement.radius;
        InvokeRepeating(nameof(SpawnObstacle), spawnInterval, spawnInterval);
    }

    private void SpawnObstacle()
    {
        if (!isCone)
        {
            Vector2 spawnPosition = GetRandomPosition();
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
            Destroy(obstacle, lifeTime + colliderActivationDelay);

            Collider2D obstacleCollider = obstacle.GetComponent<Collider2D>();
            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = false;
                StartCoroutine(ActivateObstacleCollider(obstacleCollider));
            }
        }
        else
        {
            Vector2 spawnPosition = GetRandomPosition();
            Quaternion predefinedRotation = Quaternion.Euler(0, 0, spawnAngle);
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, predefinedRotation);
            Destroy(obstacle, lifeTime);
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

            distance = Vector2.Distance(randomPosition, player.transform.position);
        } while (distance < 1f);

        return randomPosition;
    }

    private IEnumerator ActivateObstacleCollider(Collider2D obstacleCollider)
    {
        yield return new WaitForSeconds(colliderActivationDelay);
        obstacleCollider.enabled = true;
    }
}

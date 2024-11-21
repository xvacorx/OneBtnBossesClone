using System.Collections;
using UnityEngine;

public class ObstacleAttack : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] private string obstaclePoolName = "ProjectilePool";
    [SerializeField] private float obstacleSpawnInterval = 3f;
    [SerializeField] private float colliderActivationDelay = 0.5f;
    [SerializeField] private float obstacleLifeTime = 1f;
    private float spawnRadius;

    [Header("Cone Settings")]
    [SerializeField] private string conePoolName = "ProjectilePool";
    [SerializeField] private float coneSpawnInterval = 3f;
    [SerializeField] private float spawnAngle;
    [SerializeField] private float coneLifeTime = 1f;

    private GameObject player;
    private PlayerMovement movement;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        movement = player.GetComponent<PlayerMovement>();

        spawnRadius = movement.radius;

        InvokeRepeating(nameof(SpawnObstacle), obstacleSpawnInterval, obstacleSpawnInterval);
        InvokeRepeating(nameof(SpawnCone), coneSpawnInterval, coneSpawnInterval);
    }

    private void SpawnObstacle()
    {
        Vector2 spawnPosition = GetRandomPosition();
        GameObject obstacle = PoolManager.Instance.GetObject(obstaclePoolName);

        obstacle.transform.position = spawnPosition;
        obstacle.SetActive(true);

        Collider2D obstacleCollider = obstacle.GetComponent<Collider2D>();
        if (obstacleCollider != null)
        {
            obstacleCollider.enabled = false;
            StartCoroutine(ActivateObstacleCollider(obstacleCollider));
        }

        StartCoroutine(ReturnToPool(obstacle, obstacleLifeTime));
    }

    private void SpawnCone()
    {
        Vector2 spawnPosition = GetRandomPosition();
        Quaternion predefinedRotation = Quaternion.Euler(0, 0, spawnAngle);

        GameObject cone = PoolManager.Instance.GetObject(conePoolName);

        cone.transform.position = spawnPosition;
        cone.transform.rotation = predefinedRotation;
        cone.SetActive(true);

        StartCoroutine(ReturnToPool(cone, coneLifeTime));
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

    private IEnumerator ReturnToPool(GameObject obstacle, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        ObjectPool objectPool = obstacle.GetComponentInParent<ObjectPool>();
        if (objectPool != null) objectPool.ReturnObject(obstacle);
        else obstacle.SetActive(false);
    }
}
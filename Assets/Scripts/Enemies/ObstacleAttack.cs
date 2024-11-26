using System.Collections;
using UnityEngine;

public class ObstacleAttack : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] private string obstaclePoolName = "ObstaclePool";
    [SerializeField] private float obstacleSpawnInterval = 3f;
    [SerializeField] private float colliderActivationDelay = 0.5f;
    [SerializeField] private float obstacleLifeTime = 1f;

    [Header("Cone Settings")]
    [SerializeField] private string conePoolName = "ConePool";
    [SerializeField] private float coneSpawnInterval = 3f;
    [SerializeField] private float spawnAngle;
    [SerializeField] private float coneLifeTime = 1f;

    private GameObject player;
    private float spawnRadius;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        spawnRadius = movement.radius;

        InvokeRepeating(nameof(SpawnObstacle), obstacleSpawnInterval, obstacleSpawnInterval);
        InvokeRepeating(nameof(SpawnCone), coneSpawnInterval, coneSpawnInterval);
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
                StartCoroutine(ActivateObstacleCollider(obstacleCollider));
            }

            StartCoroutine(ReturnToPool(obstacle, obstacleLifeTime));
        }
    }

    private void SpawnCone()
    {
        Vector2 spawnPosition = GetRandomPosition();
        Quaternion spawnRotation = Quaternion.Euler(0, 0, spawnAngle);

        GameObject cone = GameObjectFactory.CreateObject(conePoolName, spawnPosition, spawnRotation);

        if (cone != null)
        {
            StartCoroutine(ReturnToPool(cone, coneLifeTime));
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

    private IEnumerator ActivateObstacleCollider(Collider2D collider)
    {
        yield return new WaitForSeconds(colliderActivationDelay);
        collider.enabled = true;
    }

    private IEnumerator ReturnToPool(GameObject obj, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        ObjectPool pool = obj.GetComponentInParent<ObjectPool>();
        if (pool != null)
        {
            pool.ReturnObject(obj);
        }
        else
        {
            obj.SetActive(false);
        }
    }
}

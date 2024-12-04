using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private GameObject player;
    private float spawnRadius;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        spawnRadius = movement.radius;
    }
    protected Vector2 GetRandomPosition()
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

    protected IEnumerator ActivateObstacleCollider(Collider2D collider, float colliderActivationDelay)
    {
        yield return new WaitForSeconds(colliderActivationDelay);
        collider.enabled = true;
    }

    protected IEnumerator ReturnToPool(GameObject obj, float lifeTime)
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
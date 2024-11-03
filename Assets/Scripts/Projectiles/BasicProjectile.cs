using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : ReturnToPool
{
    public Transform target;
    public float speed;
    [SerializeField] private string objectiveTag;
    [SerializeField] private float lifetime = 2f;

    private void OnEnable()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        StartCoroutine(DeactivateAfterTime(lifetime));
    }

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(objectiveTag))
        {
            if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(1);
            }
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private IEnumerator DeactivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
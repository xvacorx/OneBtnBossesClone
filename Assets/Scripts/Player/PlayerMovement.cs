using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float radius = 5f;
    public float speed = 1f;
    private float angle = 0f;
    private bool clockwise = true;
    public bool canChangeDirection = true;
    private Vector3 center;

    private bool movementEnabled = true;

    void Start()
    {
        center = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canChangeDirection)
        {
            clockwise = !clockwise;
            movementEnabled = true;
        }
        if (movementEnabled) Movement();
    }
    private void Movement()
    {
        float direction = clockwise ? -1f : 1f;
        angle += direction * speed * Time.deltaTime;

        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x + center.x, y + center.y, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")) movementEnabled = false;
    }
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(center, radius);
        }
        else
        {
            Vector3 tempCenter = Vector3.zero;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(tempCenter, radius);
        }
    }
}
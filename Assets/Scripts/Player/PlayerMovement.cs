using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float radius = 5f;
    public float speed = 1f;
    private float angle = 0f;
    private bool clockwise = true;

    private Vector3 center;

    void Start()
    {
        center = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            clockwise = !clockwise;
        }
        Movement();
    }
    private void Movement()
    {
        float direction = clockwise ? -1f : 1f;
        angle += direction * speed * Time.deltaTime;

        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x + center.x, y + center.y, 0);
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

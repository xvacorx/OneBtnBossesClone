using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float radius = 5f;
    public float speed = 1f;
    private float angle = 0f;
    private bool clockwise = true;
    private Vector3 center;
    private bool movementEnabled = true;

    public bool canChangeDirection = true;

    private PlayerControls inputActions;

    private void Awake()
    {
        inputActions = new PlayerControls();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += OnChangeDirection;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnChangeDirection;
        inputActions.Disable();
    }

    private void Start()
    {
        center = Vector3.zero;
    }

    private void Update()
    {
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

    private void OnChangeDirection(InputAction.CallbackContext context)
    {
        if (movementEnabled && canChangeDirection)
        {
            clockwise = !clockwise;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            movementEnabled = false;
        }
    }

    private void OnDrawGizmos()
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
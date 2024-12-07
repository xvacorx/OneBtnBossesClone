using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float radius = 5f;           // Radio del círculo de movimiento
    public float speed = 1f;            // Velocidad de movimiento
    private float angle = 0f;           // Ángulo actual del jugador en el círculo
    private bool clockwise = true;      // Dirección de movimiento (sentido horario o antihorario)
    private Vector3 center;             // Centro del círculo de movimiento
    private bool movementEnabled = true;

    public bool canChangeDirection = true; // ¿El jugador puede cambiar de dirección?

    private PlayerControls inputActions;
    [SerializeField] private Transform spriteTransform; // Referencia al sprite del jugador

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
        center = Vector3.zero; // Establece el centro del círculo
    }

    private void Update()
    {
        if (movementEnabled) Movement();
    }

    private void Movement()
    {
        // Determina la dirección en la que se moverá el jugador (sentido horario o antihorario)
        float direction = clockwise ? -1f : 1f;
        angle += direction * speed * Time.deltaTime;

        // Calcula la nueva posición del jugador en el círculo
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        // Actualiza la posición del jugador
        transform.position = new Vector3(x + center.x, y + center.y, 0);

        // Rota el jugador para que su parte frontal siga la dirección del movimiento
        RotatePlayer();
    }

    private void RotatePlayer()
    {

        // Calcula el ángulo de rotación para que el jugador apunte hacia la dirección del movimiento
        float rotationAngle = angle * Mathf.Rad2Deg; // Convierte de radianes a grados

        // Rota el jugador para que siempre esté mirando en la dirección de su movimiento
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAngle));
    }

    private void OnChangeDirection(InputAction.CallbackContext context)
    {
        if (canChangeDirection)
        {
            clockwise = !clockwise;
            movementEnabled = true;

            if (clockwise)
                spriteTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));  // Rotación Z=90
            else
                spriteTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));  // Rotación Z=-90
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

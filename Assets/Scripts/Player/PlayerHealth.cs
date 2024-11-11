using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private PlayerPowerUp playerPowerUp;

    void Start()
    {
        currentHealth = maxHealth;
        playerPowerUp = GetComponent<PlayerPowerUp>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (playerPowerUp != null && playerPowerUp.IsInvulnerable())
        {
            Debug.Log("El jugador es invulnerable, no recibe daño.");
            return;
        }

        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            FindObjectOfType<GameController>().Defeat();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile"))
        {
            TakeDamage(1);
            collision.gameObject.SetActive(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private PlayerPowerUp playerPowerUp;
    [SerializeField] private string lifeText;
    private GameObject lives;
    private TMP_Text livesText;

    void Start()
    {
        currentHealth = maxHealth;
        playerPowerUp = GetComponent<PlayerPowerUp>();
        lives = GameObject.Find(lifeText);
        if (lives != null) livesText = lives.GetComponent<TMP_Text>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (playerPowerUp != null && playerPowerUp.IsInvulnerable())
        {
            Debug.Log("El jugador es invulnerable, no recibe daño.");
            return;
        }
        else
        {
            currentHealth -= damageAmount;
            Debug.Log(currentHealth);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                livesText.text = $"Vidas: {currentHealth}";
                GameController.Instance.Defeat();
            }
            else livesText.text = $"Vidas: {currentHealth}";
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
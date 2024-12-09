using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Enemy : MonoBehaviour
{
    public int life;
    public void TakeDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Death();
        }
    }
    protected virtual void Death()
    {
        GameController gameController = GameController.Instance;
        if (gameController != null)
        {
            gameController.Victory();
        }
        Debug.Log("Enemy Ded");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile"))
        {
            TakeDamage(1);
            collision.gameObject.SetActive(false);
        }
    }
}

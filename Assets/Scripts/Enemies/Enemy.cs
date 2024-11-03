using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int life;

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
        FindObjectOfType<GameTimer>().StopTimer();
        GameController gameController = FindObjectOfType<GameController>();
        if (gameController != null)
        {
            gameController.Victory();
        }
        Debug.Log("Enemy Ded");
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

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
        Debug.Log("Enemy Ded");
    }
}

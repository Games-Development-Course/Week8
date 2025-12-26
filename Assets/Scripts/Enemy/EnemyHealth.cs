using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int health = 30;
    public Action onDeath;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    void Die()
    {
        onDeath?.Invoke();
        Destroy(gameObject);
    }
}

using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent<EnemyHealth>(out var enemy))
        {
            enemy.TakeDamage(10); // Assuming a fixed damage value; adjust as needed
        }
        Destroy(gameObject);
    }
}

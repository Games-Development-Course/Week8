using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool automatic;
    public float fireRate = 0.2f;
    public int damage = 10;
    public float range = 20f;

    public Transform firePoint;

    private float nextFireTime;

    public void TryFire()
    {
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireRate;
        Fire();
    }

    void Fire()
    {
        if (Physics.Raycast(
            firePoint.position,
            firePoint.forward,
            out RaycastHit hit,
            range))
        {
            if (hit.collider.TryGetComponent<EnemyHealth>(out var enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}

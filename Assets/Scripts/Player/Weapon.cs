using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public bool automatic;
    public float fireRate = 0.2f;
    public int damage = 10;
    public float range = 20f;

    [Header("References")]
    public Transform firePoint;

    [Header("Effects")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletVelocity = 30f;
    public float bulletLifetime = 3f;

    private float nextFireTime;

    public void TryFire()
    {
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireRate;
        Fire();
    }

    void Fire()
    {
        if (firePoint == null) return;

        SpawnBullet();
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
    // void Update()
    // {
    //     if(bulletPrefab == null || bulletSpawnPoint == null) return;

    //     if(automatic)
    //     {
    //         if (Input.GetKey(KeyCode.Space))
    //         {
    //             SpawnBullet();
    //         }
    //     }
    //     else
    //     {
    //         if (Input.GetKeyDown(KeyCode.Space))
    //         {
    //             SpawnBullet();
    //         }
    //     }
    // }
    void SpawnBullet()
    {
        Debug.Log("Spawning Bullet");
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = bulletSpawnPoint.forward * bulletVelocity;
        }
        Destroy(bullet, bulletLifetime);
    }
}

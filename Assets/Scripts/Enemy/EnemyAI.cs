using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Visuals")]
    public ShotTracer shotTracerPrefab;

    [Header("Stats")]
    public float attackRange = 10f;
    public float fireInterval = 1.5f;
    public int damage = 10;

    private Transform player;
    private float nextFireTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;
        float distance = dir.magnitude;

        if (distance <= attackRange)
        {
            transform.forward = dir.normalized;

            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireInterval;
                Shoot(dir.normalized);
            }
        }
    }

    void Shoot(Vector3 dir)
    {
        Vector3 start = transform.position + Vector3.up;
        Vector3 end = start + dir * attackRange;

        if (Physics.Raycast(start, dir, out RaycastHit hit, attackRange))
        {
            end = hit.point;

            if (hit.collider.TryGetComponent<PlayerHealth>(out var health))
            {
                health.TakeDamage(damage);
            }
        }

        // VISUAL TRACER
        if (shotTracerPrefab != null)
        {
            ShotTracer tracer = Instantiate(shotTracerPrefab);
            tracer.Init(start, end);
        }
    }

}

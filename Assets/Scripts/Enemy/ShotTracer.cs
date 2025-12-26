using UnityEngine;

public class ShotTracer : MonoBehaviour
{
    public float lifetime = 0.05f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Init(Vector3 start, Vector3 end)
    {
        var lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}

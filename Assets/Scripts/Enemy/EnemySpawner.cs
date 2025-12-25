using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Spawns enemies on valid maze path cells,
 * keeps distance from player,
 * and detects win condition.
 */
public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public MazeGenerator3D maze;
    public GameObject enemyPrefab;
    public Transform player;

    [Header("Spawn Settings")]
    public int enemyCount = 5;
    public float spawnHeight = 1f;
    public float minDistanceFromPlayer = 4f;

    private int aliveEnemies;

    void Start()
    {
        if (maze == null || enemyPrefab == null || player == null)
        {
            Debug.LogError("EnemySpawner not configured correctly");
            return;
        }

        SpawnEnemies();
    }

    void SpawnEnemies()
{
    List<Vector2Int> allCells = new List<Vector2Int>(maze.pathCells);
    List<Vector2Int> validCells = new List<Vector2Int>();

    foreach (var cell in allCells)
    {
        Vector3 pos = CellToWorld(cell);
        float dist = Vector3.Distance(pos, player.position);

        if (dist >= minDistanceFromPlayer)
            validCells.Add(cell);
    }

    // Fallback: maze too small → ignore distance rule
    if (validCells.Count == 0)
    {
        Debug.LogWarning(
            "No cells far enough from player. Falling back to any path cell."
        );
        validCells = allCells;
    }

    aliveEnemies = Mathf.Min(enemyCount, validCells.Count);

    for (int i = 0; i < aliveEnemies; i++)
    {
        int index = Random.Range(0, validCells.Count);
        Vector2Int cell = validCells[index];
        validCells.RemoveAt(index);

        GameObject enemy = Instantiate(
            enemyPrefab,
            CellToWorld(cell),
            Quaternion.identity
        );

        enemy.GetComponent<EnemyHealth>().onDeath += OnEnemyKilled;
    }
}

    Vector3 CellToWorld(Vector2Int cell)
    {
        return new Vector3(
            cell.x * maze.cellSize,
            spawnHeight,
            cell.y * maze.cellSize
        );
    }

    void OnEnemyKilled()
    {
        aliveEnemies--;

        if (aliveEnemies <= 0)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        Debug.Log("YOU WIN!");

        // Simple win handling (pick ONE):
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // or
        // Application.Quit();
    }
}

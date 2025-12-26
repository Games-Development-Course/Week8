using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Spawns enemies using the same logic as ResourcePlacement:
 * - Only path cells
 * - Safe from walls
 * - Respects blocked cells
 * - Keeps distance from player
 * - Tracks win condition
 */
public class EnemySpawner : MonoBehaviour
{
    public WinScreenController winScreen;

    [Header("References")]
    public MazeGenerator3D maze;
    public GameObject enemyPrefab;
    public Transform player;

    [Header("Spawn Settings")]
    public int enemyCount = 5;
    public float minDistanceFromPlayer = 4f;
    public float spawnHeight = 1f;

    private int aliveEnemies;
    private HashSet<Vector2Int> blockedCells = new();

    IEnumerator Start()
{
    if (maze == null || enemyPrefab == null || player == null)
    {
        Debug.LogError("EnemySpawner not configured correctly");
        yield break;
    }

    // WAIT until maze is fully generated
    while (!maze.IsReady)
        yield return null;

    SpawnEnemies();
}


    void SpawnEnemies()
    {
        List<Vector2Int> valid = new();

        foreach (var cell in maze.pathCells)
        {
            // Skip if already blocked
            if (blockedCells.Contains(cell))
                continue;

            // Skip if wall (defensive)
            if (maze.IsWall(cell))
                continue;

            // Skip unsafe tight corners (same as ResourcePlacement)
            if (!IsCellSafeFromWalls(maze, cell))
                continue;

            // Skip if too close to player
            Vector3 worldPos = CellToWorld(cell);
            if (Vector3.Distance(worldPos, player.position) < minDistanceFromPlayer)
                continue;

            valid.Add(cell);
        }

        // Fallback: small maze → ignore distance rule
        if (valid.Count == 0)
        {
            Debug.LogWarning(
                "EnemySpawner: no cells far enough from player. Falling back to any safe cell."
            );

            foreach (var cell in maze.pathCells)
            {
                if (maze.IsWall(cell))
                    continue;

                if (!IsCellSafeFromWalls(maze, cell))
                    continue;

                valid.Add(cell);
            }
        }

        if (valid.Count == 0)
        {
            Debug.LogError("EnemySpawner: no valid spawn cells found at all.");
            return;
        }

        Shuffle(valid);

        aliveEnemies = Mathf.Min(enemyCount, valid.Count);

        for (int i = 0; i < aliveEnemies; i++)
        {
            Vector2Int c = valid[i];

            GameObject enemy = Instantiate(
                enemyPrefab,
                CellToWorld(c),
                Quaternion.identity
            );

            blockedCells.Add(c);

            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            health.onDeath += OnEnemyKilled;
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
            WinGame();
    }

    void WinGame()
    {
        Debug.Log("YOU WIN!");

        if (winScreen != null)
            winScreen.ShowWinScreen();
    }


    // ============================================================
    // SAME SAFETY LOGIC AS ResourcePlacement
    // ============================================================

    bool IsCellSafeFromWalls(MazeGenerator3D maze, Vector2Int c)
    {
        int wallCount = 0;

        if (maze.IsWall(c + Vector2Int.right)) wallCount++;
        if (maze.IsWall(c + Vector2Int.left)) wallCount++;
        if (maze.IsWall(c + Vector2Int.up)) wallCount++;
        if (maze.IsWall(c + Vector2Int.down)) wallCount++;

        return wallCount < 3;
    }

    void Shuffle(List<Vector2Int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }
}

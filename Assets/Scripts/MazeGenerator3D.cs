using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MazeGenerator3D : MonoBehaviour
{
    public bool IsReady { get; private set; }

    [Header("Maze Settings")]
    public int width = 20;
    public int height = 20;
    public float cellSize = 2f;

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject groundPrefab;

    [Header("Runtime Data")]
    public List<Vector2Int> pathCells = new List<Vector2Int>();

    private bool[,] grid;
    private Transform wallsRoot;

    void Start()
    {
        CreateHierarchy();
        GenerateMaze();
        BuildMaze();
        CreateGround();

        IsReady = true;
    }

    // ============================================================
    // HIERARCHY
    // ============================================================
    void CreateHierarchy()
    {
        wallsRoot = new GameObject("Walls").transform;
        wallsRoot.SetParent(transform);
    }

    // ============================================================
    // MAZE GENERATION (DFS)
    // ============================================================
    void GenerateMaze()
    {
        grid = new bool[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y] = true;

        DFS(1, 1);
    }

    void DFS(int x, int y)
    {
        grid[x, y] = false;
        pathCells.Add(new Vector2Int(x, y));

        List<Vector2Int> dirs = new List<Vector2Int>
        {
            new Vector2Int(2, 0),
            new Vector2Int(-2, 0),
            new Vector2Int(0, 2),
            new Vector2Int(0, -2)
        };

        Shuffle(dirs);

        foreach (var d in dirs)
        {
            int nx = x + d.x;
            int ny = y + d.y;

            if (IsInside(nx, ny) && grid[nx, ny])
            {
                int wallX = x + d.x / 2;
                int wallY = y + d.y / 2;

                grid[wallX, wallY] = false;
                pathCells.Add(new Vector2Int(wallX, wallY));

                DFS(nx, ny);
            }
        }
    }

    bool IsInside(int x, int y)
    {
        return x > 0 && y > 0 && x < width - 1 && y < height - 1;
    }

    void Shuffle(List<Vector2Int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }

    // ============================================================
    // BUILD MAZE
    // ============================================================
    void BuildMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y])
                {
                    Vector3 pos = new Vector3(
                        x * cellSize,
                        1f,
                        y * cellSize
                    );

                    Instantiate(wallPrefab, pos, Quaternion.identity, wallsRoot);
                }
            }
        }
    }

    // ============================================================
    // GROUND
    // ============================================================
    void CreateGround()
    {
        if (groundPrefab == null) return;

        GameObject ground = Instantiate(groundPrefab);
        ground.name = "Ground";
        ground.transform.SetParent(transform);

        float groundWidth = width * cellSize;
        float groundHeight = height * cellSize;

        ground.transform.localPosition = new Vector3(
            (groundWidth / 2f) - (cellSize / 2f),
            0,
            (groundHeight / 2f) - (cellSize / 2f)
        );

        ground.transform.localScale = new Vector3(
            groundWidth / 10f,
            1,
            groundHeight / 10f
        );

        Debug.Log($"Ground Size: Width={groundWidth}, Height={groundHeight}");
    }

    // ============================================================
    // HELPERS FOR SPAWNING
    // ============================================================
    public Vector3 GetRandomPathWorldPosition(float y = 1f)
    {
        if (pathCells.Count == 0)
            return Vector3.zero;

        Vector2Int cell = pathCells[Random.Range(0, pathCells.Count)];
        return new Vector3(cell.x * cellSize, y, cell.y * cellSize);
    }
    public bool IsWall(Vector2Int cell)
{
    if (grid == null)
        return true;

    if (cell.x < 0 || cell.y < 0 || cell.x >= width || cell.y >= height)
        return true;

    return grid[cell.x, cell.y];
}


}

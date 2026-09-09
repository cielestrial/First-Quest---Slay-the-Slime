using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerController : MonoBehaviour
{
  public int moveSpeed = 1;
  public GameObject target;

  private Vector3[,,] grid;
  private float spacingX = 0.1f;
  private float spacingZ = 0.2f;
  private List<Vector2Int> path = new();
  private int pathIndex = -1;
  private Vector2Int destination;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    // StartCoroutine(MoveTo(target.transform.position));

    grid = new Vector3[8, 2, 8];
    // Initialize map
    for (int x, y = 0, maxSize = 8; y < maxSize; ++y)
    {
      for (x = 0; x < maxSize; ++x)
      {
        grid[x, 0, y] = new Vector3(x * spacingX + x, 0, y * spacingZ + y);
      }
    }

    destination = new Vector2Int(4, 3);
    bool res = FindPath(new Vector2Int(3, 0), destination);
    if (res)
      pathIndex = 0;

  }
  // Breadth-First Search
  bool FindPath(Vector2Int start, Vector2Int target)
  {

    Queue<Vector2Int> queue = new();
    HashSet<Vector2Int> visited = new();
    Dictionary<Vector2Int, Vector2Int> cameFrom = new();
    Vector2Int[] directons = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    int dirIndex = 0;
    Vector2Int adjacent;
    // procedure BFS(G, root) is

    // label root as explored
    queue.Enqueue(start);
    visited.Add(start);
    Vector2Int currentCell;
    while (queue.Count > 0)
    {
      currentCell = queue.Dequeue();
      if (currentCell == target)
      {
        ReconstructPath(cameFrom, start, target);
        return true;
      }

      for (dirIndex = 0; dirIndex < 4; ++dirIndex)
      {
        adjacent = currentCell + directons[dirIndex];
        if (visited.Contains(adjacent) || !ValidateLocation(adjacent))
          continue;

        visited.Add(adjacent);
        cameFrom[adjacent] = currentCell;
        queue.Enqueue(adjacent);
      }
    }

    return false;
  }

  void ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int target)
  {
    path.Clear();
    Vector2Int current = target;
    while (current != start)
    {
      path.Add(current);
      current = cameFrom[current];
    }
    path.Reverse();
  }

  bool ValidateLocation(Vector2 location)
  {
    if (location.x < 0 || 7 < location.x)
      return false;
    else if (location.y < 0 || 7 < location.y)
      return false;
    else
      return true;
  }

  IEnumerator MoveTo(Vector3 targetLocation)
  {
    transform.position = Vector3.MoveTowards(transform.position, targetLocation, moveSpeed * Time.deltaTime);
    yield return null;
  }

  // Update is called once per frame
  void Update()
  {
    if (pathIndex > -1)
    {
      Vector3 subTargetLocation = grid[path[pathIndex].x, 0, path[pathIndex].y];
      transform.position = Vector3.MoveTowards(transform.position, subTargetLocation, moveSpeed * Time.deltaTime);
      if (transform.position == subTargetLocation)
        ++pathIndex;
      if (transform.position == grid[destination.x, 0, destination.y])
        pathIndex = -1;
    }
  }
}

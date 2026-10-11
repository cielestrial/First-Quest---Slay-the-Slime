using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridManager", menuName = "Scriptable Objects/GridManager")]
public class GridManager : ScriptableObject
{
  public Vector3[,] LocationGrid { get; private set; }
  public int[,] DetectionGrid { get; private set; }
  public enum DetectionType
  {
    Nothing = 0,
    Ally = 1,
    Enemy = 2,
    Terrain = 3
  }
  public int Phase { get; private set; } = 1;
  public int PhaseMaxRowSize { get; private set; }
  public static readonly int MaxPhases = 3;
  // Must be an even number
  public static readonly int DefaultRowSize = 9;
  // public static readonly int RowOffset = 1;
  public static readonly int MaxRowSize = (MaxPhases * 2 - 1) * DefaultRowSize;
  public static readonly int MaxGridSize = MaxRowSize * MaxRowSize;

  // At Phase 3 its 3 to reach middle


  public static readonly float SpacingX = 0.1f;
  public static readonly float SpacingZ = 0.4f;

  void OnEnable()
  {
    // Initialize
    PhaseMaxRowSize = (Phase * 2 - 1) * DefaultRowSize;
    LocationGrid = new Vector3[MaxRowSize, MaxRowSize];
    DetectionGrid = new int[MaxRowSize, MaxRowSize];

    // Populate
    for (int x, y = 0; y < MaxRowSize; ++y)
    {
      for (x = 0; x < MaxRowSize; ++x)
      {
        LocationGrid[x, y] = new Vector3(x * (1 + SpacingX), 0, y * (1 + SpacingZ));
        DetectionGrid[x, y] = (int)DetectionType.Nothing;
      }
    }
    // For testing
    DetectionGrid[0 + DefaultRowSize * 2, 6 + DefaultRowSize * 2] = (int)DetectionType.Enemy;
  }

  public static Vector2Int ConvertIndexToVector2Int(int index)
  {
    int x = index % MaxRowSize;
    int y = (index - x) / MaxRowSize;
    return new Vector2Int(x, y);
  }

  public static Vector2Int ConvertWorldPosToGrid(Vector3 worldPos)
  {
    return new Vector2Int(
      Mathf.RoundToInt(worldPos.x / (1 + SpacingX)),
      Mathf.RoundToInt(worldPos.z / (1 + SpacingZ))
      );
  }

  // Breadth-First Search
  public List<Vector2Int> FindMovePath(Vector2Int start, Vector2Int target, bool withDetection)
  {

    Queue<Vector2Int> queue = new(MaxGridSize);
    HashSet<Vector2Int> visited = new(MaxGridSize);
    Dictionary<Vector2Int, Vector2Int> cameFrom = new(MaxGridSize);
    int dirIndex;
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
        return ReconstructPath(cameFrom, start, target);
      }

      for (dirIndex = 0; dirIndex < 4; ++dirIndex)
      {
        adjacent = currentCell + RangeManager.Cardinals2D[dirIndex];
        if (visited.Contains(adjacent) || !ValidateGridLocation(adjacent, withDetection))
          continue;

        visited.Add(adjacent);
        cameFrom[adjacent] = currentCell;
        queue.Enqueue(adjacent);
      }
    }

    return new(1);
  }

  List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int target)
  {
    List<Vector2Int> path = new(MaxGridSize);
    Vector2Int current = target;
    while (current != start)
    {
      path.Add(current);
      current = cameFrom[current];
    }
    path.Reverse();
    return path;
  }

  public bool ValidateGridLocation(Vector2Int location, bool withDetection)
  {
    if (location.x < 0 || MaxRowSize - 1 < location.x)
      return false;
    else if (location.y < 0 || MaxRowSize - 1 < location.y)
      return false;
    else if (withDetection)
    {
      if (DetectionGrid[location.x, location.y] != (int)DetectionType.Nothing)
        return false;
      else
        return true;
    }
    else
      return true;
  }

  public void NextPhase()
  {
    if (Phase + 1 <= MaxPhases)
    {
      Phase++;
      MapFactory.Instance.ScaleGround();
    }
  }

  public int[] GetLocationArrayPhase1()
  {
    // 1 * 9 = 9
    int Phase1MaxRowSize = (1 * 2 - 1) * DefaultRowSize;
    int startOffset = DefaultRowSize * 2;
    int totalOffset = (MaxRowSize * startOffset) + startOffset;
    int[] LocationArray = new int[Phase1MaxRowSize * Phase1MaxRowSize];
    for (int x, y = 0; y < Phase1MaxRowSize; ++y)
      for (x = 0; x < Phase1MaxRowSize; ++x)
        LocationArray[x + y * Phase1MaxRowSize] = totalOffset + x + y * MaxRowSize;
    return LocationArray;
  }

  public int[] GetLocationArrayPhase2()
  {
    // 3 * 9 = 27
    int Phase2MaxRowSize = (2 * 2 - 1) * DefaultRowSize;
    int Phase1MaxRowSize = (1 * 2 - 1) * DefaultRowSize;
    int totalOffset = (MaxRowSize * DefaultRowSize) + DefaultRowSize;
    int[] LocationArray = new int[Phase2MaxRowSize * Phase2MaxRowSize];
    for (int x, y = 0; y < Phase2MaxRowSize; ++y)
      for (x = 0; x < Phase2MaxRowSize; ++x)
      {
        // Skip already spawned blocks
        if (x > 8 && x < DefaultRowSize + Phase1MaxRowSize &&
            y > 8 && y < DefaultRowSize + Phase1MaxRowSize)
          continue;
        LocationArray[x + y * Phase2MaxRowSize] = totalOffset + x + y * MaxRowSize;
      }
    return LocationArray;
  }

  public int[] GetLocationArrayPhase3()
  {
    // 5 * 9 = 45
    int Phase3MaxRowSize = (3 * 2 - 1) * DefaultRowSize;
    int Phase2MaxRowSize = (2 * 2 - 1) * DefaultRowSize;
    int[] LocationArray = new int[Phase3MaxRowSize * Phase3MaxRowSize];
    for (int x, y = 0; y < Phase3MaxRowSize; ++y)
      for (x = 0; x < Phase3MaxRowSize; ++x)
      {
        // Skip already spawned blocks
        if (x > 8 && x < DefaultRowSize + Phase2MaxRowSize &&
            y > 8 && y < DefaultRowSize + Phase2MaxRowSize)
          continue;
        LocationArray[x + y * Phase3MaxRowSize] = x + y * MaxRowSize;
      }
    return LocationArray;
  }

}

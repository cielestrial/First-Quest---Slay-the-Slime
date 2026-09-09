using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
  public int Phase = 1;
  public int MaxPhases = 1;
  public GameObject Cell;
  public GameObject Parent;
  public int DefaultRowSize = 8;
  public float SpacingX = 0.1f;
  public float SpacingY = 0.2f;

  // private int cellWidth = 1;
  // private int cellHeight = 0.75
  private GameObject[,,] grid;


  private

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    // Grid = new GameObject[DefaultRowSize, DefaultRowSize, 2];
    // Initialize map
    // for (int x, y = 0, maxSize = DefaultRowSize * Phase; y < maxSize; ++y)
    // {
    //   for (x = 0; x < maxSize; ++x)
    //   {
    //     // grid[x, y, 1] = Instantiate(Cell, new Vector3(x * SpacingX + x * cellSize, 0, y * SpacingY + y * cellSize), Quaternion.identity, Parent.transform);
    //   }
    // }
    // // Initialize phase (raise cubes)
    // for (int x, y = 0, maxSize = DefaultRowSize * Phase; y < maxSize; ++y)
    // {
    //   for (x = 0; x < maxSize; ++x)
    //   {
    //     Grid[x, y, 0] = Grid[x, y, 1];
    //     Grid[x, y, 1] = null;
    //   }
    // }
  }

  // Update is called once per frame
  void Update()
  {

  }
}

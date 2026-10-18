using UnityEngine;

public class MapFactory : MonoBehaviour
{
  private GridManager gridManager;

  [SerializeField] private GameObject GroundPivotPrefab;
  public GameObject GroundPivot { get; private set; }

  [SerializeField] private Transform CellParent;
  [SerializeField] private GameObject CubeCellPrefab;
  [SerializeField] private GameObject QuadCellPrefab;

  void AdjustCellCollider()
  {
    BoxCollider collider = CubeCellPrefab.GetComponent<BoxCollider>();
    collider.size = new Vector3(1 + GridManager.SpacingX / 2, 1, 1 + GridManager.SpacingZ / 2);
    collider.center = new Vector3(GridManager.SpacingX / 4, 0, GridManager.SpacingZ / 4);
  }

  public void SetUp(GridManager gridManager)
  {
    this.gridManager = gridManager;
    AdjustCellCollider();
  }
  public void TearDown()
  {
    gridManager = null;
  }

  public void SpawnMap()
  {
    // Ground Plane
    GroundPivot = Instantiate(GroundPivotPrefab, Vector3.up, Quaternion.identity, transform);
    ScaleGround();

    // Grid
    int[] randCells = ShuffleArray(gridManager.GetLocationArrayPhase1());
    Vector2Int randIndex;
    for (int i = 0, l = randCells.Length; i < l; ++i)
    {
      randIndex = GridManager.ConvertIndexToVector2Int(randCells[i]);
      Instantiate(CubeCellPrefab, gridManager.LocationGrid[randIndex.x, randIndex.y], Quaternion.identity, CellParent);
    }
  }

  public void ScaleGround()
  {
    float x, z;
    GroundPivot.transform.position = PositionGround();
    x = gridManager.PhaseMaxRowSize * (1 + GridManager.SpacingX) - GridManager.SpacingX;
    z = gridManager.PhaseMaxRowSize * (1 + GridManager.SpacingZ) - GridManager.SpacingZ;
    GroundPivot.transform.localScale = new Vector3(x, 1, z);
  }

  private Vector3 PositionGround()
  {
    float x, z;
    x = (GridManager.MaxPhases - gridManager.Phase) * GridManager.DefaultRowSize * (1 + GridManager.SpacingX);
    z = (GridManager.MaxPhases - gridManager.Phase) * GridManager.DefaultRowSize * (1 + GridManager.SpacingZ);
    return new Vector3(x, 0, z) + RangeManager.PivotOffset;
  }

  public static int[] ShuffleArray(int[] _list)
  {
    for (int len = _list.Length - 1, rand, temp; len > 1; --len)
    {
      rand = Random.Range(0, len + 1);
      temp = _list[rand];
      _list[rand] = _list[len];
      _list[len] = temp;
    }
    return _list;
  }

}

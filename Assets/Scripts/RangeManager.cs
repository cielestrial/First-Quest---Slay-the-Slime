using UnityEngine;

public class RangeManager : MonoBehaviour
{
  public static Vector3 PivotOffset = new(-0.5f, 0f, -0.5f);
  public static readonly Vector2Int[] Cardinals2D =
    { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
  public static readonly Vector3Int[] Cardinals3D =
  { Vector3Int.forward, Vector3Int.right, Vector3Int.back, Vector3Int.left };
  private static readonly int cardinalCount = Cardinals2D.Length;
  private readonly Transform[] rangePivots = new Transform[cardinalCount];
  private int index = 0;

  void Awake()
  {
    for (index = 0; index < cardinalCount; ++index)
      rangePivots[index] = transform.GetChild(index);

    HideRanges();
  }

  public void ShowRanges(GridManager MyGridManager, Vector3 spawnLocation, Vector2Int factors)
  {
    Vector2Int center = GridManager.ConvertWorldPosToGrid(spawnLocation);
    Vector2Int cardinalGridLocation;
    Vector3 cardinalWorldLocation;
    Transform _range;
    for (index = 0; index < cardinalCount; ++index)
    {
      _range = rangePivots[index];

      cardinalGridLocation = center + Cardinals2D[index];
      if (MyGridManager.ValidateGridLocation(cardinalGridLocation, false))
      {
        cardinalWorldLocation = MyGridManager.LocationGrid[cardinalGridLocation.x, cardinalGridLocation.y];
        _range.position = cardinalWorldLocation + Vector3.Scale(PivotOffset, Cardinals3D[index]);
      }
      else
        continue;

      if (index % 2 == 0)
        ScaleHorizontalRanges(_range, factors);
      else
        ScaleVerticalRanges(_range, factors);

      ShowRange(_range);
    }

    ClipRanges();
  }

  public void HideRanges()
  {
    for (index = 0; index < cardinalCount; ++index)
      HideRange(rangePivots[index]);
  }

  private void HideRange(Transform range)
  {
    range.localScale = Vector3.one;

    range.GetChild(0).GetComponent<Renderer>().enabled = false;
    range.GetChild(0).GetComponent<BoxCollider>().enabled = false;
  }

  private void ShowRange(Transform range)
  {
    range.GetChild(0).GetComponent<Renderer>().enabled = true;
    range.GetChild(0).GetComponent<BoxCollider>().enabled = true;
  }

  private void ScaleHorizontalRanges(Transform range, Vector2Int factors)
  {
    // It is a quad
    int xSize = factors.x * 2 - 1;
    int zSize = factors.y * 2 - 1;

    float x, z;
    x = xSize * (1 + GridManager.SpacingX) - GridManager.SpacingX / 2;
    z = zSize * (1 + GridManager.SpacingZ) - GridManager.SpacingZ / 2;
    range.localScale = new Vector3(x, 1, z);
  }

  private void ScaleVerticalRanges(Transform range, Vector2Int factors)
  {
    // It is a quad
    int xSize = factors.y * 2 - 1;
    int zSize = factors.x * 2 - 1;

    float x, z;
    x = xSize * (1 + GridManager.SpacingX) - GridManager.SpacingX / 2;
    z = zSize * (1 + GridManager.SpacingZ) - GridManager.SpacingZ / 2;
    range.localScale = new Vector3(x, 1, z);
  }

  public void ClipRanges()
  {
    Transform ground = MapFactory.Instance.GroundParentInstance.transform.GetChild(0);
    ClipSetup.SetupClipBounds(ground.GetComponent<Renderer>(), gameObject);
  }

}

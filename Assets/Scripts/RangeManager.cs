using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeManager : MonoBehaviour
{
  private MapFactory mapFactory;
  [SerializeField] private GridManager MyGridManager;
  [SerializeField] private EventSystem MyEventSystem;
  public static readonly Vector3 PivotOffset = new(-0.5f, 0f, -0.5f);
  public static readonly Vector2Int[] Cardinals2D =
    { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
  public static readonly Vector3Int[] Cardinals3D =
  { Vector3Int.forward, Vector3Int.right, Vector3Int.back, Vector3Int.left };
  private static readonly int cardinalCount = Cardinals2D.Length;
  private readonly Transform[] rangePivots = new Transform[cardinalCount];

  void Awake()
  {
    mapFactory = transform.parent.GetComponent<MapFactory>();

    for (int index = 0; index < cardinalCount; ++index)
      rangePivots[index] = transform.GetChild(index);

    HideRanges();
  }
  void Start()
  {
    MyEventSystem.MyRangeEvent.AddListener(this);
  }
  void OnEnable()
  {
    MyEventSystem.MyRangeEvent.AddListener(this);
  }
  void OnDisable()
  {
    MyEventSystem.MyRangeEvent?.RemoveListener(this);
  }
  public void ClipRanges()
  {
    Transform ground = mapFactory.GroundPivot.transform.GetChild(0);
    ClipSetup.SetupClipBounds(ground.GetComponent<Renderer>(), gameObject);
  }

  public void ShowRanges(Vector3 spawnLocation, Vector2Int factors)
  {
    Vector2Int center = GridManager.ConvertWorldPosToGrid(spawnLocation);
    Vector2Int cardinalGridLocation;
    Vector3 cardinalWorldLocation;
    Transform _range;
    for (int index = 0; index < cardinalCount; ++index)
    {
      _range = rangePivots[index];

      cardinalGridLocation = center + Cardinals2D[index];
      if (GridManager.ValidGridLocation(cardinalGridLocation))
      {
        cardinalWorldLocation = MyGridManager.LocationGrid[cardinalGridLocation.x, cardinalGridLocation.y];
        _range.position = cardinalWorldLocation + Vector3.Scale(PivotOffset, Cardinals3D[index]);
      }
      else
        continue;

      ScaleRanges(_range, FactorsToSize(index, factors));
      ShowRange(_range);
    }

    ClipRanges();
  }

  public void HideRanges()
  {
    for (int index = 0; index < cardinalCount; ++index)
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

  private static Vector2Int FactorsToSize(int tagIndex, Vector2Int factors)
  => tagIndex switch
  {
    0 or 2 => new Vector2Int(factors.x * 2 - 1, factors.y * 2 - 1),
    1 or 3 => new Vector2Int(factors.y * 2 - 1, factors.x * 2 - 1),
    _ => Vector2Int.zero
  };

  private void ScaleRanges(Transform range, Vector2Int size)
  {
    float x, z;
    x = size.x * (1 + GridManager.SpacingX) - GridManager.SpacingX / 2;
    z = size.y * (1 + GridManager.SpacingZ) - GridManager.SpacingZ / 2;
    range.localScale = new Vector3(x, 1, z);
  }

  private static int TagToIndex(string tag)
  => tag switch
  {
    "North" => 0,
    "East" => 1,
    "South" => 2,
    "West" => 3,
    _ => -1,
  };

  private static Vector2Int TagIndexToAxis(int tagIndex)
  => tagIndex switch
  {
    0 or 2 => Vector2Int.up,
    1 or 3 => Vector2Int.right,
    _ => Vector2Int.zero,
  };

  private static Vector2Int TagIndexToDirection(int tagIndex)
  => tagIndex switch
  {
    0 or 1 => new Vector2Int(1, 1),
    2 => new Vector2Int(1, -1),
    3 => new Vector2Int(-1, 1),
    _ => Vector2Int.zero,
  };

  public static List<Vector2Int> GetRangeAoE(string tag, Vector2Int startPos, Vector2Int factors)
  {
    int tagIndex = TagToIndex(tag);
    if (tagIndex == -1)
      return new(0);

    return ScaleAndClipGridRanges(
      startPos,
      FactorsToSize(tagIndex, factors),
      TagIndexToAxis(tagIndex) * factors,
      TagIndexToDirection(tagIndex)
      );
  }

  private static List<Vector2Int> ScaleAndClipGridRanges(
    Vector2Int startPos,
    Vector2Int size,
    Vector2Int axis,
    Vector2Int direction
    )
  {

    List<Vector2Int> coverage = new(size.x * size.y);
    Vector2Int cell;
    for (int y = 0; y < size.y; ++y)
      for (int x = 0; x < size.x; ++x)
      {
        cell = (new Vector2Int(x, y) - axis) * direction + startPos;
        if (GridManager.ValidGridLocation(cell))
          coverage.Add(cell);
      }
    return coverage;
  }


}

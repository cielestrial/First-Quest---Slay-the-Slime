using UnityEngine;

public class MapFactory : MonoBehaviour
{
  [SerializeField] private GridManager MyGridManager;

  [SerializeField] private GameObject GroundPivotPrefab;
  public GameObject GroundPivot { get; private set; }

  [SerializeField] private Transform CellParent;
  [SerializeField] private GameObject CubeCellPrefab;
  [SerializeField] private GameObject QuadCellPrefab;

  [SerializeField] private GameObject HeroPrefab;
  [SerializeField] private HeroPartyStatus HeroParty;
  [SerializeField] private GameObject SlimePrefab;
  [SerializeField] private SlimePartyStatus SlimeParty;


  void Awake()
  {
    BoxCollider collider = CubeCellPrefab.GetComponent<BoxCollider>();
    collider.size = new Vector3(1 + GridManager.SpacingX / 2, 1, 1 + GridManager.SpacingZ / 2);
    collider.center = new Vector3(GridManager.SpacingX / 4, 0, GridManager.SpacingZ / 4);
  }

  void OnEnable()
  {
    MyGridManager.AttachMapFactory(this);
  }

  void Start()
  {
    // Ground Plane
    GroundPivot = Instantiate(GroundPivotPrefab, Vector3.up, Quaternion.identity, transform);
    ScaleGround();

    // Grid
    int[] randCells = ShuffleArray(MyGridManager.GetLocationArrayPhase1());
    Vector2Int randIndex;
    for (int i = 0, l = randCells.Length; i < l; ++i)
    {
      randIndex = GridManager.ConvertIndexToVector2Int(randCells[i]);
      Instantiate(CubeCellPrefab, MyGridManager.LocationGrid[randIndex.x, randIndex.y], Quaternion.identity, CellParent);
    }

    // Slime
    Vector2Int slimeStartL = SlimeParty.GetStartLocation();
    Quaternion slimeStartQ = SlimeParty.GetStartQuaternion();
    GameObject slime = Instantiate(SlimePrefab, MyGridManager.LocationGrid[slimeStartL.x, slimeStartL.y], slimeStartQ);
    SlimeParty.AddUnit(slime.GetComponent<SlimeController>());
    // Hero
    Vector2Int heroStartL = HeroParty.GetStartLocation();
    Quaternion heroStartQ = HeroParty.GetStartQuaternion();
    GameObject hero = Instantiate(HeroPrefab, MyGridManager.LocationGrid[heroStartL.x, heroStartL.y], heroStartQ);
    HeroParty.AddUnit(hero.GetComponent<KnightController>());
  }

  public void ScaleGround()
  {
    float x, z;
    GroundPivot.transform.position = PositionGround();
    x = MyGridManager.PhaseMaxRowSize * (1 + GridManager.SpacingX) - GridManager.SpacingX;
    z = MyGridManager.PhaseMaxRowSize * (1 + GridManager.SpacingZ) - GridManager.SpacingZ;
    GroundPivot.transform.localScale = new Vector3(x, 1, z);
  }

  private Vector3 PositionGround()
  {
    float x, z;
    x = (GridManager.MaxPhases - MyGridManager.Phase) * GridManager.DefaultRowSize * (1 + GridManager.SpacingX);
    z = (GridManager.MaxPhases - MyGridManager.Phase) * GridManager.DefaultRowSize * (1 + GridManager.SpacingZ);
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

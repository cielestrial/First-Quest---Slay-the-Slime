using UnityEngine;

public class MapFactory : MonoBehaviour
{
  public static MapFactory Instance { get; private set; }

  [SerializeField] private GridManager MyGridManager;

  [SerializeField] private GameObject GroundParent;
  public GameObject GroundParentInstance { get; private set; }

  [SerializeField] private Transform CellParent;
  [SerializeField] private GameObject CubeCell;
  [SerializeField] private GameObject QuadCell;

  [SerializeField] private GameObject Hero;
  [SerializeField] private HeroPartyStatus MyHeroParty;
  [SerializeField] private GameObject Slime;
  [SerializeField] private SlimePartyStatus MySlimeParty;


  void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }
    Instance = this;
    BoxCollider collider = CubeCell.GetComponent<BoxCollider>();
    collider.size = new Vector3(1 + GridManager.SpacingX / 2, 1, 1 + GridManager.SpacingZ / 2);
    collider.center = new Vector3(GridManager.SpacingX / 4, 0, GridManager.SpacingZ / 4);
  }

  void Start()
  {
    // Ground Plane
    GroundParentInstance = Instantiate(GroundParent, Vector3.up, Quaternion.identity, transform);
    ScaleGround();

    // Grid
    int[] randCells = ShuffleList(MyGridManager.GetLocationArrayPhase1());
    Vector2Int randIndex;
    for (int i = 0, l = randCells.Length; i < l; ++i)
    {
      randIndex = GridManager.ConvertIndexToVector2Int(randCells[i]);
      Instantiate(CubeCell, MyGridManager.LocationGrid[randIndex.x, randIndex.y], Quaternion.identity, CellParent);
    }

    // Slime
    Vector2Int slimeStartL = MySlimeParty.GetStartLocation();
    Quaternion slimeStartQ = MySlimeParty.GetStartQuaternion();
    GameObject slime = Instantiate(Slime, MyGridManager.LocationGrid[slimeStartL.x, slimeStartL.y], slimeStartQ);
    MySlimeParty.SetSlime(slime.GetComponent<SlimeController>());
    // Hero
    Vector2Int heroStartL = MyHeroParty.GetStartLocation();
    Quaternion heroStartQ = MyHeroParty.GetStartQuaternion();
    GameObject hero = Instantiate(Hero, MyGridManager.LocationGrid[heroStartL.x, heroStartL.y], heroStartQ);
    MyHeroParty.SetKnight(hero.GetComponent<KnightController>());
  }

  public void ScaleGround()
  {
    float x, z;
    GroundParentInstance.transform.position = PositionGround();
    x = MyGridManager.PhaseMaxRowSize * (1 + GridManager.SpacingX) - GridManager.SpacingX;
    z = MyGridManager.PhaseMaxRowSize * (1 + GridManager.SpacingZ) - GridManager.SpacingZ;
    GroundParentInstance.transform.localScale = new Vector3(x, 1, z);
  }

  private Vector3 PositionGround()
  {
    float x, z;
    x = (GridManager.MaxPhases - MyGridManager.Phase) * GridManager.DefaultRowSize * (1 + GridManager.SpacingX);
    z = (GridManager.MaxPhases - MyGridManager.Phase) * GridManager.DefaultRowSize * (1 + GridManager.SpacingZ);
    return new Vector3(x, 0, z) + RangeManager.PivotOffset;
  }

  static int[] ShuffleList(int[] _list)
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

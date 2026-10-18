using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
  [SerializeField] private Camera mainCameraPrefab;
  [SerializeField] private Light mainLightingPrefab;
  [SerializeField] private Volume globalVolumePrefab;
  [SerializeField] private EventSystem eventSystemPrefab;

  [SerializeField] private EventBus eventBus;
  [SerializeField] private GridManager gridManager;
  [SerializeField] private TargetingSystem targetingSystem;
  [SerializeField] private BattleSystem battleSystem;
  [SerializeField] private SlimePartyStatus slimeParty;
  [SerializeField] private HeroPartyStatus heroParty;

  [SerializeField] private MapFactory mapPrefab;
  [SerializeField] private UIManager canvasPrefab;
  [SerializeField] private TurnManager turnManagerPrefab;

  private Scene battleScene;
  private Camera mainCamera;
  private MapFactory mapFactory;
  private UIManager uiManager;
  private TurnManager turnManager;
  private RangeManager rangeManager;
  private bool isShuttingDown;

  async void Start()
  {
    SafeStart();

    await StartPhase1();
    StartPhase2();
  }
  void OnDestroy() => Shutdown();

  void SafeStart()
  {
    Shutdown();
    isShuttingDown = false;
  }

  public static T SpawnInScene<T>(T prefab, Scene scene) where T : Component
  {
    T instance = Instantiate(prefab);
    SceneManager.MoveGameObjectToScene(instance.gameObject, scene);
    return instance;
  }

  async Task StartPhase1()
  {
    // Bootstrapper scene is 0
    // TitleScreen scene is 1
    // Battle scene is 2 
    await SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    await SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
    battleScene = SceneManager.GetSceneByName("Battle");

    // Scene defaults
    mainCamera = SpawnInScene(mainCameraPrefab, battleScene);
    Instantiate(mainLightingPrefab, battleScene);
    Instantiate(globalVolumePrefab, battleScene);
    Instantiate(eventSystemPrefab, battleScene);

    // Scriptable Objects
    eventBus.SetUp();
    gridManager.CreateGrid();

    targetingSystem.SetUp(mainCamera, eventBus);

    slimeParty.SetUp(eventBus, gridManager, targetingSystem);
    heroParty.SetUp(eventBus, gridManager, targetingSystem);

    battleSystem.SetUp(eventBus, slimeParty, heroParty);

    await Task.Delay(1000);
    await SceneManager.UnloadSceneAsync(1);

    // Monobehaviours
    mapFactory = SpawnInScene(mapPrefab, battleScene);
    mapFactory.SetUp(gridManager);
    mapFactory.SpawnMap();

    uiManager = SpawnInScene(canvasPrefab, battleScene);
    uiManager.SetUp(eventBus);

    turnManager = SpawnInScene(turnManagerPrefab, battleScene);
    turnManager.SetUp(eventBus, slimeParty, heroParty);

    rangeManager = mapFactory.transform.GetChild(1).GetComponent<RangeManager>();
    rangeManager.SetUp(mapFactory, eventBus, gridManager);
  }
  void StartPhase2()
  {
    // Game ready
    eventBus.Snapshot();
    heroParty.IsPlaying = true;
    slimeParty.SpawnInitialUnit(battleScene);
    heroParty.SpawnInitialUnit(battleScene);
    turnManager.StartBattle();
  }

  void Shutdown()
  {
    if (isShuttingDown) return;
    isShuttingDown = true;
    ShutdownPhase1();
    ShutdownPhase2();
  }
  void ShutdownPhase1()
  {
    // Game
    if (turnManager != null)
      turnManager.StopBattle();
    if (gridManager != null)
      gridManager.ResetDetectionGrid();
  }

  void ShutdownPhase2()
  {
    // Monobehaviours
    if (rangeManager != null)
    {
      rangeManager.TearDown();
      rangeManager = null;
    }

    if (turnManager != null)
    {
      turnManager.TearDown();
      turnManager = null;
    }

    if (uiManager != null)
    {
      uiManager.TearDown();
      uiManager = null;
    }

    if (mapFactory != null)
    {
      mapFactory.TearDown();
      mapFactory = null;
    }

    // Scriptable Objects
    if (battleSystem != null)
      battleSystem.TearDown();

    if (heroParty != null)
      heroParty.TearDown();
    if (slimeParty != null)
      slimeParty.TearDown();

    if (targetingSystem != null)
      targetingSystem.TearDown();

    if (eventBus != null)
      eventBus.TearDown();

    // Scene defaults
    mainCamera = null;
  }
}
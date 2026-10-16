using System.Threading.Tasks;
using Unity.VisualScripting;
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
  [SerializeField] private SlimePartyStatus slimeParty;
  [SerializeField] private HeroPartyStatus heroParty;

  [SerializeField] private MapFactory mapPrefab;
  [SerializeField] private Canvas canvasPrefab;
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

    slimeParty.IsPlaying = false;
    heroParty.IsPlaying = false;

    await StartPhase1();
    await StartPhase2();
  }
  void OnDestroy() => Shutdown();

  void SafeStart()
  {
    Shutdown();
    isShuttingDown = false;
  }
  async Task StartPhase1()
  {
    // Bootstrapper scene is 0
    // TitleScreen scene is 1
    // Battle scene is 2 
    await SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
    battleScene = SceneManager.GetSceneByName("Battle");

    // Scene defaults
    mainCamera = Instantiate(mainCameraPrefab, battleScene).GetComponent<Camera>();
    Instantiate(mainLightingPrefab, battleScene);
    Instantiate(globalVolumePrefab, battleScene);
    Instantiate(eventSystemPrefab, battleScene);

    // Scriptable Objects
    eventBus.AddEvents();
    gridManager.CreateGrid();

    targetingSystem.AddDependencies(mainCamera, eventBus);
    targetingSystem.AddListener();

    slimeParty.AddDependencies(eventBus, gridManager, targetingSystem);
    heroParty.AddDependencies(eventBus, gridManager, targetingSystem);

    // Monobehaviours
    mapFactory = Instantiate(mapPrefab, battleScene).GetComponent<MapFactory>();
    mapFactory.AddDependencies(gridManager);
    mapFactory.AdjustCellCollider();
    mapFactory.SpawnMap();

    uiManager = Instantiate(canvasPrefab, battleScene).GetComponent<UIManager>();
    uiManager.AddDependencies(eventBus);
    uiManager.AddListener();

    turnManager = Instantiate(turnManagerPrefab, battleScene).GetComponent<TurnManager>();
    turnManager.AddDependencies(eventBus, slimeParty, heroParty);
    turnManager.AddListener();

    rangeManager = mapFactory.transform.GetChild(1).GetComponent<RangeManager>();
    rangeManager.AddDependencies(mapFactory, eventBus, gridManager);
    rangeManager.Setup();
    rangeManager.AddListener();
  }
  async Task StartPhase2()
  {
    // Game ready
    heroParty.IsPlaying = true;
    // Maybe move to different file?
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
    turnManager.StopBattle();
    slimeParty.IsPlaying = false;
    heroParty.IsPlaying = false;
  }
  void ShutdownPhase2()
  {
    // Monobehaviours
    rangeManager.RemoveListener();
    rangeManager.ClearDependencies();
    rangeManager = null;

    turnManager.RemoveListener();
    turnManager.ClearDependencies();
    turnManager = null;

    uiManager.RemoveListener();
    uiManager.ClearDependencies();
    uiManager = null;

    mapFactory.ClearDependencies();
    mapFactory = null;

    // Scriptable Objects
    heroParty.ClearDependencies();
    slimeParty.ClearDependencies();

    targetingSystem.RemoveListener();
    targetingSystem.ClearDependencies();

    eventBus.ClearEvents();

    // Scene defaults
    mainCamera = null;
  }
}
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "HeroPartyStatus", menuName = "Scriptable Objects/HeroPartyStatus")]
public class HeroPartyStatus : PartyStatus<HeroController>
{
  [SerializeField] private KnightController KnightPrefab;
  // [SerializeField] private MageController MagePrefab;
  // [SerializeField] private ArcherController ArcherPrefab;
  // [SerializeField] private RogueController RoguePrefab;

  public override void SpawnInitialUnit(Scene scene)
  {
    Vector2Int knightStartL = GetStartLocation();
    Quaternion knightStartQ = GetStartQuaternion();
    KnightController knight = Bootstrapper.SpawnInScene(KnightPrefab, scene);
    knight.SetUp(eventBus, gridManager, GetDetectionType(), targetingSystem);
    knight.transform.SetPositionAndRotation(gridManager.LocationGrid[knightStartL.x, knightStartL.y], knightStartQ);
    gridManager.AddToDetectionGrid(knightStartL, knight);
    AddUnit(knight);
    SetInitialTurnEnder(knight);
  }
}
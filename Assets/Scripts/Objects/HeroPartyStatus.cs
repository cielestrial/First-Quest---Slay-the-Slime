using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "HeroPartyStatus", menuName = "Scriptable Objects/HeroPartyStatus")]
public class HeroPartyStatus : PartyStatus<HeroController>
{
  [SerializeField] private KnightController KnightPrefab;
  [SerializeField] private MageController MagePrefab;
  [SerializeField] private ArcherController ArcherPrefab;
  [SerializeField] private RogueController RoguePrefab;

  public override void SpawnInitialUnit(Scene scene)
  {
    Vector2Int knightStartL = GetStartLocation();
    Quaternion knightStartQ = GetStartQuaternion();
    KnightController knight = Instantiate(KnightPrefab, scene).GetComponent<KnightController>();
    knight.AddDependencies(eventBus, gridManager, targetingSystem);
    knight.Setup();
    knight.transform.SetPositionAndRotation(gridManager.LocationGrid[knightStartL.x, knightStartL.y], knightStartQ);
    AddUnit(knight);
  }
}
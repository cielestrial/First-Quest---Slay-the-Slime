using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SlimePartyStatus", menuName = "Scriptable Objects/SlimePartyStatus")]
public class SlimePartyStatus : PartyStatus<SlimeController>
{
  [SerializeField] private SlimeController SlimePrefab;

  public override void SpawnInitialUnit(Scene scene)
  {
    Vector2Int slimeStartL = GetStartLocation();
    Quaternion slimeStartQ = GetStartQuaternion();
    SlimeController slime = Bootstrapper.SpawnInScene(SlimePrefab, scene);
    slime.SetUp(eventBus, gridManager, GetDetectionType(), targetingSystem);
    slime.transform.SetPositionAndRotation(gridManager.LocationGrid[slimeStartL.x, slimeStartL.y], slimeStartQ);
    gridManager.AddToDetectionGrid(slimeStartL, slime);
    AddUnit(slime);
    SetInitialTurnEnder(slime);
  }

  public void AIUnitSelection()
  {
    if (Units.Count >= 0)
      eventBus.MyTurnEvent.UnitSelected(Units[0]);
  }
}

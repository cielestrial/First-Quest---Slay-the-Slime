using Unity.VisualScripting;
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
    SlimeController slime = Instantiate(SlimePrefab, scene).GetComponent<SlimeController>();
    slime.AddDependencies(eventBus, gridManager, targetingSystem);
    slime.Setup();
    slime.transform.SetPositionAndRotation(gridManager.LocationGrid[slimeStartL.x, slimeStartL.y], slimeStartQ);
    AddUnit(slime);
  }
}

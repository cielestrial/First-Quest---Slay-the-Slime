using System.Collections.Generic;
using Grid.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleSystem", menuName = "Scriptable Objects/BattleSystem")]
public class BattleSystem : ScriptableObject
{
  private EventBus eventBus;
  private SlimePartyStatus slimeParty;
  private HeroPartyStatus heroParty;

  public void SetUp(
    EventBus eventBus,
    SlimePartyStatus slimeParty,
    HeroPartyStatus heroParty)
  {
    this.eventBus = eventBus;
    this.slimeParty = slimeParty;
    this.heroParty = heroParty;
    AddListener();
  }
  public void TearDown()
  {
    RemoveListener();
    heroParty = null;
    slimeParty = null;
    eventBus = null;
  }
  void AddListener()
  {
    eventBus.MyBattleEvent?.AddListener(this);
  }
  void RemoveListener()
  {
    if (eventBus != null)
      eventBus.MyBattleEvent?.RemoveListener(this);
  }

  public void DamageDealt(UnitController by, float with, List<Vector2Int> to)
  {
    int K = 30;
    int attackerLv = by.CurrentLevel;
    UnitController victim;

    foreach (Vector2Int location in to)
    {
      victim = by.MyGridManager.DetectionGrid[location];
      float dmgRed = victim.Defence / (victim.Defence + K + attackerLv * 2);
      float dmg = by.Attack * with * (1 - dmgRed);
      victim.TakeDamage((int)dmg);
    }
  }

  public void UnitDeath(UnitController deceased)
  {
    if ((slimeParty.IsPlaying && deceased.DetectionType == DetectionType.Ally) ||
    (heroParty.IsPlaying && deceased.DetectionType == DetectionType.Enemy))
      slimeParty.StoreDeadUnit(deceased as SlimeController);
    else
      heroParty.StoreDeadUnit(deceased as HeroController);
  }

}
